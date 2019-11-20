﻿using PlaygroundsGallery.Domain.DTOs;
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
		private bool UploadPhotoToPhotoLibrary(
			PhotoDto photoToReturnDto,
			PhotoForCreationDto photoForCreationDto)
		{
			var uploadedPhotoToReturn = _photoUploader.UploadPhoto(photoForCreationDto.File);
			if (uploadedPhotoToReturn.UploadSucceeded)
			{
				photoToReturnDto.Url = uploadedPhotoToReturn?.Uri?.ToString();
				photoToReturnDto.PublicId = uploadedPhotoToReturn?.PublicId;
			}

			return uploadedPhotoToReturn.UploadSucceeded;
		}        
		
		public async Task<PhotoDto> UploadPhoto(PhotoForCreationDto photoForCreationDto)
        {
			if (photoForCreationDto.File == null || photoForCreationDto.File.Length < 1)
			{
				throw new PhotoUploadFileEmptyException();
			}
			
			var photoToReturnDto = _mapper.Map<PhotoDto>(photoForCreationDto);
			var uploadSucceeded = UploadPhotoToPhotoLibrary(photoToReturnDto, photoForCreationDto);
			if (uploadSucceeded == false)
			{
				throw new PhotoUploadToLibraryException();
			}
			
			var photoToAdd = _mapper.Map<Photo>(photoToReturnDto);
			await _photoRepository.Add(photoToAdd);

			return photoToReturnDto;
        }

		public async Task<PhotoDto> UpdatePhoto(PhotoDto photoToUpdateDto)
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
				take: count);
			return _mapper.Map<IEnumerable<PhotoDto>>(photos);
		}

        public async Task<PhotoDto> GetPhoto(int id) 
			=> _mapper.Map<PhotoDto>(await _photoRepository.Get(id));

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
