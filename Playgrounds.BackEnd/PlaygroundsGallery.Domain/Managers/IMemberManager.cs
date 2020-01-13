using System.Threading.Tasks;
using PlaygroundsGallery.Domain.DTOs;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IMemberManager
    {
        Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> Register(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> GetMember(int id);    
    }
}