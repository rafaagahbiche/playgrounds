using System.Threading.Tasks;
using Photos.Services.DTOs;

namespace Photos.Services.Managers
{
    public interface IPhotoMember
    {
        Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto);
        Task<bool> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto);
        Task<bool> DeletePhoto(int photoId);
        Task<string> DeletePhotoPhysically(int photoId);
    }
}