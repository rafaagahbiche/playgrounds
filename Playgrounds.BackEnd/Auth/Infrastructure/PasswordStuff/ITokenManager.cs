namespace Auth.Infrastructure.PasswordStuff
{
    public interface ITokenManager
    {
         string CreateToken(int userId, string userName, string profilePictureUrl);
    }
}