using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Auth.Infrastructure.Exceptions;
using Auth.Infrastructure.PasswordStuff;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public partial class MemberManager : IMemberManager
    {
        private readonly GalleryContext _context;
        // private readonly IRepository<Member> _memberRepository;
        private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IMapper _mapper;       
        private readonly ILogger<MemberManager> _logger; 

        public MemberManager(
            GalleryContext context,
            // IRepository<Member> memberRepository,
            IPasswordManager passwordManager, 
            ITokenManager tokenManager,
            IMapper mapper,
            ILogger<MemberManager> logger)
        {
            this._context = context;
            // this._memberRepository = memberRepository;
            this._passwordManager = passwordManager;
            this._tokenManager = tokenManager;
            this._mapper = mapper;
            this._logger = logger;        
        }

        private MemberEntityDto SetHachPasswordToMember(MemberToLoginDto memberToLogin)
        {
            var memberEntityDto = _mapper.Map<MemberEntityDto>(memberToLogin);
            var encryptedPassword = _passwordManager.SetPasswordHashAndSalt(memberToLogin.Password);
            memberEntityDto.PasswordHash = encryptedPassword.PasswordHash;
            memberEntityDto.PasswordSalt = encryptedPassword.PasswordSalt;
            return memberEntityDto;
        }
        private async Task<bool> DoesMemberExist(MemberToLoginDto memberToLoginDto)
		{
			return await this._context.Members.AnyAsync(m => 
                (!string.IsNullOrEmpty(m.LoginName) 
                    && !string.IsNullOrEmpty(memberToLoginDto.LoginName) 
                    && m.LoginName.Equals(memberToLoginDto.LoginName, StringComparison.CurrentCultureIgnoreCase))
				    || (!string.IsNullOrEmpty(m.EmailAddress) 
                        && !string.IsNullOrEmpty(memberToLoginDto.EmailAddress) 
                        && m.EmailAddress.Equals(memberToLoginDto.EmailAddress, StringComparison.CurrentCultureIgnoreCase)));
		}

        public async Task<MemberDto> GetMember(int id) 
            => _mapper.Map<MemberDto>(await this._context.Members.FindAsync(id));
        
        private async Task<Member> GetMember(string loginName, string emailAddress)
        {
            var userName = !string.IsNullOrEmpty(loginName) ? loginName : emailAddress;
            var memberEntity = await this._context.Members
                    .Include(m => m.ProfilePictures)
                    .SingleOrDefaultAsync(m => ((!string.IsNullOrEmpty(m.LoginName) 
                                                    && m.LoginName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                                                || (!string.IsNullOrEmpty(m.EmailAddress) 
                                                    && m.EmailAddress.Equals(userName, StringComparison.CurrentCultureIgnoreCase))));

            return memberEntity;
        }

        public async Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto)
        {
            MemberLoggedInDto memberLoggedInDto = null;
            var memberEntity = await GetMember(memberToLoginDto.LoginName, memberToLoginDto.EmailAddress);
            if (memberEntity != null)
            {
                var memberEntityDto = _mapper.Map<MemberEntityDto>(memberEntity);
                memberLoggedInDto = _mapper.Map<MemberLoggedInDto>(memberEntity);
                if (_passwordManager.VerifyPasswordHash(
                    memberToLoginDto.Password, 
                    memberEntityDto.PasswordHash, 
                    memberEntityDto.PasswordSalt))
                {
                    memberLoggedInDto.Token = _tokenManager.CreateToken(
                        memberLoggedInDto.Id, 
                        memberLoggedInDto.LoginName, 
                        memberLoggedInDto.ProfilePictureUrl);
                }

                this._logger.LogInformation("Wrong password.", memberToLoginDto);
            }
            
            // If this clause is reached it means the member was not found or the password is wrong.
            this._logger.LogInformation("Member was not found.", memberToLoginDto);
            return memberLoggedInDto;
        }

        private async Task<bool> AddMemberEntity(Member memberEntity)
        {
            bool memberAdded = false;
            try 
            {
                memberEntity.Created = DateTime.UtcNow;
                await this._context.Members.AddAsync(memberEntity);
                memberAdded = await this._context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error occurred while adding member entity.");
            }

            return memberAdded;
        } 
        
        public async Task<MemberDto> Register(MemberToLoginDto memberToLoginDto)
        {
            MemberDto newMember = null;
            if (!await this.DoesMemberExist(memberToLoginDto))
            {
                var memberEntityDto = SetHachPasswordToMember(memberToLoginDto);
                var memberEntity = _mapper.Map<Member>(memberEntityDto);
                if (await AddMemberEntity(memberEntity))
                {
                    newMember = _mapper.Map<MemberDto>(memberEntity);
                }
            }
            else 
            {
                this._logger.LogCritical("The Member entity to register already exist.", memberToLoginDto);
                // throw new MemberCreationException(memberToLoginDto.LoginName, memberToLoginDto.EmailAddress);
            }

            return newMember;
        }
    }
}