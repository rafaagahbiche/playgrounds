using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
namespace PlaygroundsGallery.Helper
{

    public class CloudinaryPhotoUploader : IPhotoUploader
    {
        public IAccountSettings UploaderAccount { get; set; }

        private Cloudinary _cloudinaryAccount;

        public CloudinaryPhotoUploader(IAccountSettings uploaderAccount)
        {
            UploaderAccount = uploaderAccount;
        }

        private void CreateCloudinaryAcount()
        {
            _cloudinaryAccount = new Cloudinary(
                new Account(
                    UploaderAccount.Name,
                    UploaderAccount.ApiKey,
                    UploaderAccount.ApiSecret));
        }

        public UploadedPhotoToReturn UploadPhoto(IFormFile file)
        {
            var uploadedPhotoToReturn = new UploadedPhotoToReturn();
			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new ImageUploadParams()
				{
					File = new FileDescription(file.Name, stream)
                    //,Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
				};
                
                CreateCloudinaryAcount();
                if (_cloudinaryAccount != null)
    			{	
                    var uploadResult = _cloudinaryAccount.Upload(uploadParams);
                    if (uploadResult != null) 
                    {
                        uploadedPhotoToReturn.PublicId = uploadResult.PublicId;
                        uploadedPhotoToReturn.Uri = uploadResult.SecureUri;
                        uploadedPhotoToReturn.UploadSucceeded = true;
                    }
                }
			}

			return uploadedPhotoToReturn;        
        }

        public bool DeletePhoto()
        {
            return false;
        }
    }

    public class UploadedPhotoToReturn
    {
        public Uri Uri { get; set; }

        public string PublicId { get; set; }

        public bool UploadSucceeded { get; set; } = false;
    }
}