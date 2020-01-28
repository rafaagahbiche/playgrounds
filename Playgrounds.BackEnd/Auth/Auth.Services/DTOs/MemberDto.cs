namespace Auth.Services
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string LoginName { get; set; }   
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}