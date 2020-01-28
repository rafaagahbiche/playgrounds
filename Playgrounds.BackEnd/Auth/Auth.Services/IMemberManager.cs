using System.Threading.Tasks;

namespace Auth.Services
{
    public interface IMemberManager
    {
        Task<MemberLoggedInDto> Login(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> Register(MemberToLoginDto memberToLoginDto);
        Task<MemberDto> GetMember(int id);    
    }
}