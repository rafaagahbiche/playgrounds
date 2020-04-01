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
using Microsoft.Extensions.Logging;

namespace Photos.Services.Managers
{
    public partial class PhotoManager : IPhotoManager
	{
		private readonly IRepository<models.Photo> _photoRepository;
		private readonly IMapper _mapper;
        private readonly ILogger<PhotoManager> _logger;
		public PhotoManager(
			IRepository<models.Photo> photoRepository, 
			IMapper mapper,
            ILogger<PhotoManager> logger)
		{
			_photoRepository = photoRepository;
			_mapper = mapper;
			_logger = logger;
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
				predicate : p => p.Deleted == false && p.PlaygroundId == playgroundId,
				orderBy: q => q.OrderByDescending(p => p.Created), 
				includeProperties: new Expression<Func<models.Photo, object>>[] {
					p => p.Member
				} 
			));

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
	}
}
