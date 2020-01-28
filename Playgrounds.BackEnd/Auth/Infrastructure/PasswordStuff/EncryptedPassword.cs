namespace Auth.Infrastructure.PasswordStuff
{
    public class EncryptedPassword
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}