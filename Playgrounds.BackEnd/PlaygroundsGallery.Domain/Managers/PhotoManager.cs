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
		public async Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto)
        {
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
		
		public async Task<IEnumerable<PhotoDto>> GetPhotosByPlayground(int playgroundId)
			=> _mapper.Map<IEnumerable<PhotoDto>>(await _photoRepository.Find(p => p.Deleted == false && p.PlaygroundId == playgroundId));

		public async Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId)
		{
			return _mapper.Map<IEnumerable<PhotoAsPostDto>>(await _photoRepository.Find(
				predicate: p => p.Deleted == false && p.PlaygroundId == playgroundId && p.Member != null,
				orderBy: q => q.OrderByDescending(p => p.Created), 
				includeProperties: new Expression<Func<Photo, object>>[] { 
						(p => p.Member), 
						(p => p.Member.ProfilePictures ) 
					})
				);
		}
			
		public async Task<bool> DeletePhoto(string publicId)
		{
			var deletionSucceeded = false;
			if (!string.IsNullOrEmpty(publicId))
			{
				Photo photoToDelete = null;
				photoToDelete = await _photoRepository.SingleOrDefault(p => p.PublicId == publicId);
				if (photoToDelete != null)
				{
					photoToDelete.Deleted = true;
					deletionSucceeded = await _photoRepository.Update(photoToDelete);				
				}
				else 
				{
					throw new PhotoNotFoundException(publicId);
				}
			}
			
			return deletionSucceeded;
		}

		public async Task<bool> DeletePhotoPhysically(string publicId)
		{
			var deletionSucceeded = false;
			if (!string.IsNullOrEmpty(publicId))
			{
				var photoToDelete = await _photoRepository.SingleOrDefault(p => p.PublicId == publicId);
				if (photoToDelete != null)
				{
					deletionSucceeded = await _photoRepository.Remove(photoToDelete);
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
