using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Photos.Services.DTOs;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using Microsoft.EntityFrameworkCore;

namespace Photos.Services.Managers
{
    public partial class PhotoManager : IPhotoManager
	{
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PhotoManager> _logger;
		public PhotoManager(
			GalleryContext context,
			IMapper mapper,
            ILogger<PhotoManager> logger)
		{
			
			 this._context = context;
			this._mapper = mapper;
			this._logger = logger;
		}

		public async Task<IEnumerable<PhotoDto>> GetRecentPhotos(int count)
		{
			var recentPhotoDtos = Enumerable.Empty<PhotoDto>();
			try 
			{
				var photosEntities = await _context.Photos
							.Include(p => p.Member)
							.ThenInclude(m => m.ProfilePictures)
							.Where(p => p.Deleted == false)
							.OrderByDescending(p => p.Created)
							.Take(count)
							.ToListAsync();
				recentPhotoDtos = _mapper.Map<IEnumerable<PhotoDto>>(photosEntities);
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error occurred searching for recent photos.");
			}

			return recentPhotoDtos;
		}

        public async Task<PhotoDto> GetPhoto(int id) 
		{
			PhotoDto photoDto = null;
			try
			{
				var photoEntity = await _context.Photos
										.Include(ph => ph.Playground)
										.ThenInclude(p => p.Location)
										.SingleOrDefaultAsync(ph => ph.Id == id);

				photoDto = _mapper.Map<PhotoDto>(photoEntity);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred searching for photo by id.");
			}
			
			return photoDto;
		}

        public async Task<IEnumerable<PhotoDto>> GetPhotosByMemberId(int id) 
		{
			var photosDtos = Enumerable.Empty<PhotoDto>();
			try
			{
				var photosEntities = await _context.Photos
					.Where(p => p.Deleted == false && p.MemberId == id)
					.OrderByDescending(p => p.Created)
					.ToListAsync();

				photosDtos = _mapper.Map<IEnumerable<PhotoDto>>(photosEntities);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred searching for photos by member id.");
			}
			
			return photosDtos;
		}
		
		public async Task<IEnumerable<PhotoDto>> GetPhotosByPlayground(int playgroundId)
		{
			var photosDtos = Enumerable.Empty<PhotoDto>();
			try
			{
				var photosEntities = await _context.Photos
					.Include(p => p.Member)
					.ThenInclude(m => m.ProfilePictures)
					.Where(p => p.Deleted == false && p.PlaygroundId == playgroundId)
					.OrderByDescending(p => p.Created)
					.ToListAsync();

				photosDtos = _mapper.Map<IEnumerable<PhotoDto>>(photosEntities);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred searching for photos by playground id.");
			}
			
			return photosDtos;
		}
			
		public async Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId)
		{
			var photosDtos = Enumerable.Empty<PhotoAsPostDto>();
			try
			{
				var photosEntities = await _context.Photos
					.Include(p => p.Member)
					.ThenInclude(m => m.ProfilePictures)
					.Where(p => p.Deleted == false && p.PlaygroundId == playgroundId)
					.OrderByDescending(p => p.Created)
					.ToListAsync();

				photosDtos = _mapper.Map<IEnumerable<PhotoAsPostDto>>(photosEntities);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred searching for photos by playground id.");
			}
			
			return photosDtos;
		}
	}
}
