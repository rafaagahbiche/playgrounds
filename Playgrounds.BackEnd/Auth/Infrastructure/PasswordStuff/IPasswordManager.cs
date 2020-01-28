namespace Auth.Infrastructure.PasswordStuff
{
    public interface IPasswordManager
    {
        EncryptedPassword SetPasswordHashAndSalt(string password);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}