using Microsoft.AspNetCore.Http;

namespace Photos.Infrastructure.Uploader
{
    public interface IPhotoUploader
    {
        IAccountSettings UploaderAccount { get; set; }
        UploadedPhotoToReturn UploadPhoto(IFormFile file);
        bool DeletePhoto(string publicId);
    }
}