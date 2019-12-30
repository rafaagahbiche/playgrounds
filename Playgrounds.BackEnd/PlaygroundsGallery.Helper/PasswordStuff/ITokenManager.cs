namespace PlaygroundsGallery.Helper
{
    public interface ITokenManager
    {
         string CreateToken(int userId, string userName, string profilePictureUrl);
    }
}