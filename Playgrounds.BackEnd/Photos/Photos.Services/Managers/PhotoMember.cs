using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Photos.Services.DTOs;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;

namespace Photos.Services.Managers
{
    public class PhotoMember: IPhotoMember
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PhotoMember> _logger;
		public PhotoMember(
            GalleryContext context,
			IMapper mapper,
            ILogger<PhotoMember> logger)
		{
            this._context = context;
			_mapper = mapper;
			_logger = logger;
		}
        public async Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto)
        {
            PhotoInsertedDto photoInsertedDto = null;
			try
			{
                var photoToAdd = _mapper.Map<Photo>(photoToInsertDto);
				await _context.Photos.AddAsync(photoToAdd);
                if (await _context.SaveChangesAsync() > 0)
                {
                    photoInsertedDto = _mapper.Map<PhotoInsertedDto>(photoToAdd);
                }
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred when adding a photo entity.");
			}

			return photoInsertedDto;
        }

		public async Task<bool> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto)
		{
            bool updateSucceeded = false;
            try
            {
                var photoToUpdate = _mapper.Map<Photo>(photoToUpdateDto);
                photoToUpdate.Updated = DateTime.UtcNow;
                _context.Photos.Update(photoToUpdate);
                updateSucceeded = await _context.SaveChangesAsync() > 0;
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
                var photoToDelete = _context.Photos.Find(photoId);
                if (photoToDelete == null)
                {
                    _logger.LogError($"Delete Photo: Photo with id {photoId} was not found.");
                }
                else
                {
                    photoToDelete.Deleted = true;
                    photoToDelete.Updated = DateTime.UtcNow;
                    _context.Photos.Update(photoToDelete);				
                    deletionSucceeded = await _context.SaveChangesAsync() > 0;
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
                var photoToDelete = _context.Photos.Find(photoId);
                if (photoToDelete == null)
                {
                    _logger.LogError($"Delete Photo physically: Photo with id {photoId} was not found.");
                }
                else
                {
                    photoPublicId = photoToDelete.PublicId;
                    _context.Remove(photoToDelete);
                    deletionSucceeded = await _context.SaveChangesAsync() > 0;
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