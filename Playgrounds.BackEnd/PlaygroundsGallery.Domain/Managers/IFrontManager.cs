using PlaygroundsGallery.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IFrontManager
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
