using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Helper.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundsGallery.Domain.Managers
{
    public partial class FrontManager : IFrontManager
    {
		private bool UploadPhotoToPhotoLibrary(PhotoForCreationDto photoForCreationDto)
		{
			var uploadedPhotoToReturn = _photoUploader.UploadPhoto(photoForCreationDto.File);
			if (uploadedPhotoToReturn.UploadSucceeded)
			{
				photoForCreationDto.Url = uploadedPhotoToReturn?.Uri?.ToString();
				photoForCreationDto.PublicId = uploadedPhotoToReturn?.PublicId;
				photoForCreationDto.UploadDate = DateTime.Now;
			}

			return uploadedPhotoToReturn.UploadSucceeded;
		}        
		
		public async Task<PhotoToReturnDto> UploadPhoto(PhotoForCreationDto photoForCreationDto)
        {
			if (photoForCreationDto.File == null || photoForCreationDto.File.Length < 1)
			{
				throw new PhotoUploadFileEmptyException();
			}

			var uploadSucceeded = UploadPhotoToPhotoLibrary(photoForCreationDto);
			if (uploadSucceeded == false)
			{
				throw new PhotoUploadToLibraryException();
			}
			
			var photoToAdd = _mapper.Map<Photo>(photoForCreationDto);
			await _photoRepository.Add(photoToAdd);

			return _mapper.Map<PhotoToReturnDto>(photoForCreationDto);
        }

		public async Task<IEnumerable<PhotoToReturnDto>> GetRecentPhotos(int count)
		{
			var photos = await _photoRepository.Find(orderBy: q => q.OrderByDescending(p => p.UploadDate), take: count);
			return _mapper.Map<IEnumerable<PhotoToReturnDto>>(photos);
		}

        public async Task<PhotoToReturnDto> GetPhoto(int id) 
			=> _mapper.Map<PhotoToReturnDto>(await _photoRepository.Get(id));

        public async Task<IEnumerable<PhotoToReturnDto>> GetPhotosByMemberId(int id) 
			=> _mapper.Map<IEnumerable<PhotoToReturnDto>>(await _photoRepository.Find(p => p.MemberId == id));
    }
}
