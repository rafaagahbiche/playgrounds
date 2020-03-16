using Microsoft.AspNetCore.Http;

namespace Photos.Infrastructure.Uploader
{
    public interface IPhotoUploader
    {
        // CloudinarySettings UploaderAccount { get; set; }
        UploadedPhotoToReturn UploadPhoto(IFormFile file);
        bool DeletePhoto(string publicId);
    }
}