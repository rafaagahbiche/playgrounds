using Photos.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Photos.Services.Managers
{
    public interface IPhotoManager
    {
        Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto);
        Task<PhotoDto> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto);
        Task<bool> DeletePhoto(string publicId);
        Task<bool> DeletePhotoPhysically(string publicId);
        Task<PhotoDto> GetPhoto(int id);
        Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id);
        Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count);
        Task<IEnumerable<PhotoDto>> GetPhotosByPlayground(int playgroundId);
        Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId);
    }
}
