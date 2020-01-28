namespace Photos.Services.Managers
{
    using Photos.Services.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostManager
    {
        Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId);
        // Task<IEnumerable<CheckinAsPostDto>> GetCheckinsAsPostsByPlaygroundId(int playgroundId);
        // Task<IEnumerable<PostDto>> GetPostsByPlaygroundId(int playgroundId);
    }
}