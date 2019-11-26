using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Helper.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace PlaygroundsGallery.Domain.Managers
{
    public partial class FrontManager : IFrontManager
    {
		private bool UploadPhotoToPhotoLibrary(
			PhotoToInsertDto photoToInsertDto,
			PhotoToUploadDto photoToUploadDto)
		{
			var uploadedPhotoToReturn = _photoUploader.UploadPhoto(photoToUploadDto.File);
			if (uploadedPhotoToReturn.UploadSucceeded)
			{
				photoToInsertDto.Url = uploadedPhotoToReturn?.Uri?.ToString();
				photoToInsertDto.PublicId = uploadedPhotoToReturn?.PublicId;
			}

			return uploadedPhotoToReturn.UploadSucceeded;
		}        
		
		public async Task<PhotoInsertedDto> UploadPhoto(PhotoToUploadDto photoToUploadDto)
        {
			if (photoToUploadDto.File == null || photoToUploadDto.File.Length < 1)
			{
				throw new PhotoUploadFileEmptyException();
			}
			
			var photoToInsertDto = _mapper.Map<PhotoToInsertDto>(photoToUploadDto);
			var uploadSucceeded = UploadPhotoToPhotoLibrary(photoToInsertDto, photoToUploadDto);
			if (uploadSucceeded == false)
			{
				throw new PhotoUploadToLibraryException();
			}
			
			var photoToAdd = _mapper.Map<Photo>(photoToInsertDto);
			await _photoRepository.Add(photoToAdd);

			return _mapper.Map<PhotoInsertedDto>(photoToAdd);
        }

		public async Task<PhotoDto> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto)
		{
			var photoToUpdate = _mapper.Map<Photo>(photoToUpdateDto);
			if (await _photoRepository.Update(photoToUpdate))
			{
				return _mapper.Map<PhotoDto>(photoToUpdate);
			}
			else
			{
				throw new PhotoUpdateException();
			}
		}

		public async Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count)
		{
			var photos = await _photoRepository.Find(
				predicate: p => p.Deleted == false, 
				orderBy: q => q.OrderByDescending(p => p.Created), 
				includeProperties: new Expression<Func<Photo, object>>[] { (p => p.Playground), (p => p.Playground.Location)},
				take: count);
			return _mapper.Map<IEnumerable<PhotoDto>>(photos);
		}

        public async Task<PhotoDto> GetPhoto(int id) 
		{
			var photo = (await _photoRepository.SingleOrDefault(
				predicate: p => p.Id == id, 
				includeProperties: new Expression<Func<Photo, object>>[] { (p => p.Playground), (p => p.Playground.Location)}
			));
			
			return _mapper.Map<PhotoDto>(photo);
		}
        public async Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id) 
			=> _mapper.Map<IEnumerable<PhotoDto>>(await _photoRepository.Find(p => p.Deleted == false && p.MemberId == id));

		public async Task<bool> DeletePhoto(string publicId, bool physically)
		{
			var deletionSucceeded = false;
			if (!string.IsNullOrEmpty(publicId))
			{
				Photo photoToDelete = null;
				try
				{
					photoToDelete = await _photoRepository.SingleOrDefault(p => p.PublicId == publicId);
				}
				catch
				{
					throw new PhotoNotFoundException(publicId);
				}
				if (photoToDelete != null)
				{
					photoToDelete.Deleted = true;
					deletionSucceeded = await _photoRepository.Update(photoToDelete);
					if (physically) 
					{
						deletionSucceeded = await _photoRepository.Remove(photoToDelete) && _photoUploader.DeletePhoto(publicId);
					}					
				}
				else 
				{
					throw new PhotoNotFoundException(publicId);
				}
			}
			
			return deletionSucceeded;
		}
    }
}
