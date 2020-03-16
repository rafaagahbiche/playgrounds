using Photos.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Photos.Services.Managers
{
    public interface IPhotoManager
    {
        Task<PhotoDto> GetPhoto(int id);
        Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id);
        Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count);
        Task<IEnumerable<PhotoDto>> GetPhotosByPlayground(int playgroundId);
        Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId);
    }
}
