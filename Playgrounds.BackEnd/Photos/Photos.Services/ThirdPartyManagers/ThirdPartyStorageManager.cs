using AutoMapper;
using Photos.Services.DTOs;
using Photos.Infrastructure.Exceptions;
using Photos.Infrastructure.Uploader;

namespace Photos.Services.ThirdPartyManagers
{
    public class ThirdPartyStorageManager : IThirdPartyStorageManager
    {
        private readonly IPhotoUploader _photoUploader;
        private readonly IMapper _mapper;
        public ThirdPartyStorageManager(IPhotoUploader photoUploader, IMapper mapper)
        {
            this._photoUploader = photoUploader;
            this._mapper = mapper;
        }
        
        public PhotoToInsertDto UploadPhoto(PhotoToUploadDto photoToUploadDto)
		{
            PhotoToInsertDto photoToInsertDto = null;
            if (photoToUploadDto.File == null || photoToUploadDto.File.Length < 1)
			{
				throw new PhotoUploadFileEmptyException();
			}
			var uploadedPhotoToReturn = _photoUploader.UploadPhoto(photoToUploadDto.File);
			if (uploadedPhotoToReturn.UploadSucceeded)
			{
                photoToInsertDto = this._mapper.Map<PhotoToInsertDto>(photoToUploadDto);
				photoToInsertDto.Url = uploadedPhotoToReturn?.Uri?.ToString();
				photoToInsertDto.PublicId = uploadedPhotoToReturn?.PublicId;
			}

			return photoToInsertDto;
		}

        public bool DeletePhoto(string publicId)
        {
            return _photoUploader.DeletePhoto(publicId);
        }  
    }
}