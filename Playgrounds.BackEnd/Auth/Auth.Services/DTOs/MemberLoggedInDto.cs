namespace Auth.Services
{
    public class MemberLoggedInDto: MemberDto
    {
        public string Token { get; set; }   
    }
}