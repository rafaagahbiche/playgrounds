using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
namespace Photos.Infrastructure.Uploader
{
    public class CloudinaryPhotoUploader : IPhotoUploader
    {
        public IAccountSettings UploaderAccount { get; set; }

        private Cloudinary _cloudinaryAccount;

        private string _envFolder;

        public CloudinaryPhotoUploader(IAccountSettings uploaderAccount, string envFolder)
        {
            UploaderAccount = uploaderAccount;
            _envFolder = envFolder;
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
					File = new FileDescription(file.Name, stream),
                    Folder = _envFolder
                    //,Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
				};
                
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

        public bool DeletePhoto(string publicId)
        {
            var deletionSucceeded = false;
            if (_cloudinaryAccount != null)
            {	
                var deleteResult = _cloudinaryAccount.DeleteResources(new string[] {publicId});
                deletionSucceeded = deleteResult.Deleted.ContainsValue("deleted");
            }

            return deletionSucceeded;
        }
    }

    public class UploadedPhotoToReturn
    {
        public Uri Uri { get; set; }

        public string PublicId { get; set; }

        public bool UploadSucceeded { get; set; } = false;
    }
}