using PlaygroundsGallery.Domain.DTOs;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IThirdPartyStorageManager
    {
        PhotoToInsertDto UploadPhoto(PhotoToUploadDto photoToUploadDto);
        bool DeletePhoto(string publicId);
    }
}