using System;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Helper.Exceptions;

namespace PlaygroundsGallery.Domain.Managers
{
    public partial class FrontManager : IFrontManager
    {
        public async Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto)
        {
            Member memberEntity = null;
            var userName = !string.IsNullOrEmpty(memberToLoginDto.LoginName) ? memberToLoginDto.LoginName : memberToLoginDto.EmailAddress;
            try 
            {
                memberEntity = await _memberRepository.SingleOrDefault(
                    predicate:
                    m => ((!string.IsNullOrEmpty(m.LoginName) && m.LoginName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                        || (!string.IsNullOrEmpty(m.EmailAddress) && m.EmailAddress.Equals(userName, StringComparison.CurrentCultureIgnoreCase))),
                    includeProperties: m => m.ProfilePictures);
            }
            catch (Exception)
            {
                // TO DO Log actual exception
				throw new MemberLoginException();
            }

            if (memberEntity == null)
			{
				throw new MemberLoginException();
			}

			if (!_passwordManager.VerifyPasswordHash(memberToLoginDto.Password, memberEntity.PasswordHash, memberEntity.PasswordSalt))
			{
				throw new MemberLoginException();
			}
            
            MemberLoggedInDto memberLoggedInDto = _mapper.Map<MemberLoggedInDto>(memberEntity);
            memberLoggedInDto.Token = _tokenManager.CreateToken(
                memberEntity.Id, 
                memberEntity.LoginName, 
                memberLoggedInDto.ProfilePictureUrl);
                
            return memberLoggedInDto;
        }

        public async Task<bool> MemberExists(string loginName, string emailAddress)
		{
			return await _memberRepository.AnyAsync(m => 
                (!string.IsNullOrEmpty(m.LoginName) && !string.IsNullOrEmpty(loginName) 
                    && m.LoginName.Equals(loginName, StringComparison.CurrentCultureIgnoreCase))
				|| (!string.IsNullOrEmpty(m.EmailAddress) && !string.IsNullOrEmpty(emailAddress) 
                    && m.EmailAddress.Equals(emailAddress, StringComparison.CurrentCultureIgnoreCase)));
		}

        public async Task<MemberDto> Register(MemberToLoginDto memberToLoginDto)
        {
            MemberDto newMember = null;
            if (!await MemberExists(memberToLoginDto.LoginName, memberToLoginDto.EmailAddress))
			{
                var memberEntity = _mapper.Map<Member>(memberToLoginDto);
			 	_passwordManager.SetPasswordHashAndSalt(memberToLoginDto.Password);
				memberEntity.PasswordHash = _passwordManager.PasswordHash;
				memberEntity.PasswordSalt = _passwordManager.PasswordSalt;
				if (await _memberRepository.Add(memberEntity))
                {
                    newMember = _mapper.Map<MemberDto>(memberEntity);
                }
			}
            else
            {
                throw new MemberCreationException(memberToLoginDto.LoginName, memberToLoginDto.EmailAddress);
            }

            return newMember;
        }

        public async Task<MemberDto> GetMember(int id) 
            => _mapper.Map<MemberDto>(await _memberRepository.Get(id));
    }
}