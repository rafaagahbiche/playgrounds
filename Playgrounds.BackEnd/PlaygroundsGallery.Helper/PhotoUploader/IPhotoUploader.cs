using Microsoft.AspNetCore.Http;

namespace PlaygroundsGallery.Helper
{
    public interface IPhotoUploader
    {
        IAccountSettings UploaderAccount { get; set; }
        UploadedPhotoToReturn UploadPhoto(IFormFile file);
        bool DeletePhoto();
    }
}