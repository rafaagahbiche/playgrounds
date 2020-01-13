namespace PlaygroundsGallery.Domain.DTOs
{
    public class MemberEntityDto : MemberToLoginDto
    {
        public int Id { get; set; }
        public byte[] PasswordSalt { get; set; }
		public byte[] PasswordHash { get; set; }
    }
}