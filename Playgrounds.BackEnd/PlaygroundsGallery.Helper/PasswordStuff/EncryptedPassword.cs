namespace PlaygroundsGallery.Helper
{
    public class EncryptedPassword
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}