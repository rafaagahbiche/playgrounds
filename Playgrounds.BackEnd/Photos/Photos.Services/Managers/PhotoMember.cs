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

        public async Task<bool> DeletePhoto(string publicId)
		{
			bool deletionSucceeded = false;
			if (!string.IsNullOrEmpty(publicId))
			{
                try
                {
                    var photoToDelete = await _photoRepository.SingleOrDefault(p => p.PublicId == publicId);
                    photoToDelete.Deleted = true;
                    deletionSucceeded = await _photoRepository.Update(photoToDelete);				
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured when setting Photo entity as deleted.");
                }
			}
			
			return deletionSucceeded;
		}

		public async Task<bool> DeletePhotoPhysically(string publicId)
		{
			bool deletionSucceeded = false;
			if (!string.IsNullOrEmpty(publicId))
			{
                try
                {
                    var photoToDelete = await _photoRepository.SingleOrDefault(p => p.PublicId == publicId);
                    deletionSucceeded = await _photoRepository.Remove(photoToDelete);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured when deleting Photo entity.");
                }
			}
			
			return deletionSucceeded;
		}
    }
}