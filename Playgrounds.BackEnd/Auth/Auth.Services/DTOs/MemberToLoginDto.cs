namespace Auth.Services
{
    public class MemberToLoginDto
    {
        public string LoginName { get; set; }   
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}