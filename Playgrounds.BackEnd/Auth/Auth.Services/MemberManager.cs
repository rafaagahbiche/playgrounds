using System;
using System.Threading.Tasks;
using AutoMapper;
using Auth.Infrastructure.Exceptions;
using Auth.Infrastructure.PasswordStuff;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;
using Microsoft.Extensions.Logging;

namespace Auth.Services
{
    public partial class MemberManager : IMemberManager
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IMapper _mapper;       
        private readonly ILogger<MemberManager> _logger; 

        public MemberManager(
            IRepository<Member> memberRepository,
            IPasswordManager passwordManager, 
            ITokenManager tokenManager,
            IMapper mapper,
            ILogger<MemberManager> logger)
        {
            this._memberRepository = memberRepository;
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
			return await _memberRepository.AnyAsync(m => 
                (!string.IsNullOrEmpty(m.LoginName) 
                    && !string.IsNullOrEmpty(memberToLoginDto.LoginName) 
                    && m.LoginName.Equals(memberToLoginDto.LoginName, StringComparison.CurrentCultureIgnoreCase))
				    || (!string.IsNullOrEmpty(m.EmailAddress) 
                        && !string.IsNullOrEmpty(memberToLoginDto.EmailAddress) 
                        && m.EmailAddress.Equals(memberToLoginDto.EmailAddress, StringComparison.CurrentCultureIgnoreCase)));
		}

        public async Task<MemberDto> GetMember(int id) 
                    => _mapper.Map<MemberDto>(await _memberRepository.Get(id));
        
        private async Task<Member> GetMember(string loginName, string emailAddress)
        {
            var userName = !string.IsNullOrEmpty(loginName) ? loginName : emailAddress;
            var memberEntity = await _memberRepository.SingleOrDefault(
                predicate:
                m => ((!string.IsNullOrEmpty(m.LoginName) && m.LoginName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                    || (!string.IsNullOrEmpty(m.EmailAddress) && m.EmailAddress.Equals(userName, StringComparison.CurrentCultureIgnoreCase))),
                includeProperties: m => m.ProfilePictures);

            return memberEntity;
        }

        public async Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto)
        {
            var memberEntity = await GetMember(memberToLoginDto.LoginName, memberToLoginDto.EmailAddress);
            if (memberEntity != null)
            {
                var memberEntityDto = _mapper.Map<MemberEntityDto>(memberEntity);
                var memberLoggedInDto = _mapper.Map<MemberLoggedInDto>(memberEntity);
                if (_passwordManager.VerifyPasswordHash(
                    memberToLoginDto.Password, 
                    memberEntityDto.PasswordHash, 
                    memberEntityDto.PasswordSalt))
                {
                    memberLoggedInDto.Token = _tokenManager.CreateToken(
                        memberLoggedInDto.Id, 
                        memberLoggedInDto.LoginName, 
                        memberLoggedInDto.ProfilePictureUrl);
                    return memberLoggedInDto;
                }

                this._logger.LogInformation("Wrong password.", memberToLoginDto);
            }
            
            // If this clause is reached it means the member was not found or the password is wrong.
            this._logger.LogInformation("Member was not found.", memberToLoginDto);
            throw new MemberLoginException();
        }

        public async Task<MemberDto> Register(MemberToLoginDto memberToLoginDto)
        {
            MemberDto newMember = null;
            if (!await this.DoesMemberExist(memberToLoginDto))
            {
                var memberEntityDto = SetHachPasswordToMember(memberToLoginDto);
                var memberEntity = _mapper.Map<Member>(memberEntityDto);
                try 
                {
                    if (await _memberRepository.Add(memberEntity))
                    {
                        newMember = _mapper.Map<MemberDto>(memberEntity);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "Error occurred while adding member entity.");
                }
            }
            else 
            {
                this._logger.LogCritical("The Member entity to add already exist.", memberToLoginDto);
            }

            return newMember;
        }
    }
}