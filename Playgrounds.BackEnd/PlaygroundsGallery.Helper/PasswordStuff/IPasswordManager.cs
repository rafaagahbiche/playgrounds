namespace PlaygroundsGallery.Helper
{
    public interface IPasswordManager
    {
        byte[] PasswordHash { get; set; }
        
        byte[] PasswordSalt { get; set; }

        void SetPasswordHashAndSalt(string password);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}