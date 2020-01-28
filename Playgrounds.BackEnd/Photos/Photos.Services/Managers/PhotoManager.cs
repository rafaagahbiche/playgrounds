using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using models = PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;
using Photos.Infrastructure.Exceptions;
using Photos.Services.DTOs;

namespace Photos.Services.Managers
{
    public partial class PhotoManager : IPhotoManager
	{
		private readonly IRepository<models.Photo> _photoRepository;
		private readonly IMapper _mapper;

		public PhotoManager(
			IRepository<models.Photo> photoRepository, 
			IMapper mapper)
		{
			_photoRepository = photoRepository;
			_mapper = mapper;
		}

		public async Task<PhotoInsertedDto> AddPhoto(PhotoToInsertDto photoToInsertDto)
        {
			var photoToAdd = _mapper.Map<models.Photo>(photoToInsertDto);
			await _photoRepository.Add(photoToAdd);
			return _mapper.Map<PhotoInsertedDto>(photoToAdd);
        }

		public async Task<PhotoDto> UpdatePhoto(PhotoToUpdateDto photoToUpdateDto)
		{
			var photoToUpdate = _mapper.Map<models.Photo>(photoToUpdateDto);
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
				includeProperties: new Expression<Func<models.Photo, object>>[] { (p => p.Playground), (p => p.Playground.Location)},
				take: count);

			return _mapper.Map<IEnumerable<PhotoDto>>(photos);
		}

        public async Task<PhotoDto> GetPhoto(int id) 
		{
			var photo = (await _photoRepository.SingleOrDefault(
				predicate: p => p.Id == id, 
				includeProperties: new Expression<Func<models.Photo, object>>[] { (p => p.Playground), (p => p.Playground.Location)}
			));
			
			return _mapper.Map<PhotoDto>(photo);
		}

        public async Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id) 
			=> _mapper.Map<IEnumerable<PhotoDto>>(await _photoRepository.Find(p => p.Deleted == false && p.MemberId == id));
		
		public async Task<IEnumerable<PhotoDto>> GetPhotosByPlayground(int playgroundId)
			=> _mapper.Map<IEnumerable<PhotoDto>>(await _photoRepository.Find(
				p => p.Deleted == false && p.PlaygroundId == playgroundId));

		public async Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId)
		{
			return _mapper.Map<IEnumerable<PhotoAsPostDto>>(await _photoRepository.Find(
				predicate: p => p.Deleted == false && p.PlaygroundId == playgroundId && p.Member != null,
				orderBy: q => q.OrderByDescending(p => p.Created), 
				includeProperties: new Expression<Func<models.Photo, object>>[] { 
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
				models.Photo photoToDelete = null;
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
