using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Helper;
using PlaygroundsGallery.Helper.Exceptions;

namespace PlaygroundsGallery.Domain.Managers
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