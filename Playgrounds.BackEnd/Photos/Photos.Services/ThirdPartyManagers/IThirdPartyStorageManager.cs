using Photos.Services.DTOs;

namespace Photos.Services.ThirdPartyManagers
{
    public interface IThirdPartyStorageManager
    {
        PhotoToInsertDto UploadPhoto(PhotoToUploadDto photoToUploadDto);
        bool DeletePhoto(string publicId);
    }
}