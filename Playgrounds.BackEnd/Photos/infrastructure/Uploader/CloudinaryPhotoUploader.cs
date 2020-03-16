using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Photos.Infrastructure.Uploader
{
    public class CloudinaryPhotoUploader : IPhotoUploader
    {
        // public CloudinarySettings UploaderAccount { get; set; }

        private Cloudinary _cloudinaryAccount;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly ILogger _logger;

        public CloudinaryPhotoUploader(
            IOptions<CloudinarySettings> uploaderAccount, 
            ILogger<CloudinaryPhotoUploader> logger)
        {
            _cloudinaryConfig = uploaderAccount;
            _logger = logger;
            try 
            {
                _cloudinaryAccount = new Cloudinary(
                    new Account(
                        _cloudinaryConfig.Value.Name,
                        _cloudinaryConfig.Value.ApiKey,
                        _cloudinaryConfig.Value.ApiSecret));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Cloudinary account.");
            }
        }

        public UploadedPhotoToReturn UploadPhoto(IFormFile file)
        {
            var uploadedPhotoToReturn = new UploadedPhotoToReturn();
			using (var stream = file.OpenReadStream())
			{
				var uploadParams = new ImageUploadParams()
				{
					File = new FileDescription(file.Name, stream),
                    Folder = _cloudinaryConfig.Value.FolderName
                    //,Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
				};
                
                if (_cloudinaryAccount != null)
    			{	
                    try
                    {
                        var uploadResult = _cloudinaryAccount.Upload(uploadParams);
                        if (uploadResult != null) 
                        {
                            uploadedPhotoToReturn.PublicId = uploadResult.PublicId;
                            uploadedPhotoToReturn.Uri = uploadResult.SecureUri;
                            uploadedPhotoToReturn.UploadSucceeded = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occured while uploading photos to Cloudinary.");
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
                try
                {
                    var deleteResult = _cloudinaryAccount.DeleteResources(new string[] {publicId});
                    deletionSucceeded = deleteResult.Deleted.ContainsValue("deleted");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured while deleting photos to Cloudinary.");
                }
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