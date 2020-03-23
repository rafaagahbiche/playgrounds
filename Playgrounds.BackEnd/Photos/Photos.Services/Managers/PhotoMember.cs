using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Photos.Services.DTOs;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Photos.Services.Managers
{
    public class PhotoMember: IPhotoMember
    {
		private readonly IRepository<Photo> _photoRepository;
		private readonly IMapper _mapper;
        private readonly ILogger<PhotoMember> _logger;
		public PhotoMember(
			IRepository<Photo> photoRepository, 
			IMapper mapper,
            ILogger<PhotoMember> logger)
		{
			_photoRepository = photoRepository;
			_mapper = mapper;
			_logger = logger;
		}
        public async Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto)
        {
			var photoToAdd = _mapper.Map<Photo>(photoToInsertDto);
			try
			{
				await _photoRepository.Add(photoToAdd);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred when adding a photo entity.");
			}

			return _mapper.Map<PhotoInsertedDto>(photoToAdd);
        }

		public async Task<bool> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto)
		{
            bool updateSucceeded = false;
            try
            {
                var photoToUpdate = _mapper.Map<Photo>(photoToUpdateDto);
                updateSucceeded = await _photoRepository.Update(photoToUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured when updating Photo entity.");
            }

            return updateSucceeded;
		}

        public async Task<bool> DeletePhoto(int photoId)
		{
			bool deletionSucceeded = false;
            try
            {
                var photoToDelete = await _photoRepository.Get(photoId);
                if (photoToDelete == null)
                {
                    _logger.LogError($"Delete Photo: Photo with id {photoId} was not found.");
                }
                else
                {
                    photoToDelete.Deleted = true;
                    deletionSucceeded = await _photoRepository.Update(photoToDelete);				
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured when setting Photo entity as deleted.");
            }
			
			return deletionSucceeded;
		}

		public async Task<string> DeletePhotoPhysically(int photoId)
		{
			bool deletionSucceeded = false;
            var photoPublicId = string.Empty;
            try
            {
                var photoToDelete = await _photoRepository.Get(photoId);
                if (photoToDelete == null)
                {
                    _logger.LogError($"Delete Photo physically: Photo with id {photoId} was not found.");
                }
                else
                {
                    photoPublicId = photoToDelete.PublicId;
                    deletionSucceeded = await _photoRepository.Remove(photoToDelete);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured when deleting Photo entity.");
            }
			
			return photoPublicId;
		}
    }
}