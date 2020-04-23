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
    public class PostManager : IPostManager
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PhotoMember> _logger;
        public PostManager(
            GalleryContext context,
			IMapper mapper,
            ILogger<PhotoMember> logger)
        {
            this._context = context;
			this._mapper = mapper;
			this._logger = logger;
        }

        public async Task<IEnumerable<PhotoAsPostDto>> GetPhotosAsPostByPlayground(int playgroundId)
        {
            var photosAsPostDto = Enumerable.Empty<PhotoAsPostDto>();
            try 
            {
                var photosEntities = await _context.Photos
                        .Include(p => p.Member)
                        .ThenInclude(m => m.ProfilePictures)
                        .Where(p => p.Deleted == false && p.PlaygroundId == playgroundId && p.Member != null)
                        .OrderByDescending(p => p.Created)
                        .ToListAsync();
                photosAsPostDto = _mapper.Map<IEnumerable<PhotoAsPostDto>>(photosEntities);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Exception while searching for photos by playground id.");
            }

            return photosAsPostDto;
        }

        // public async Task<IEnumerable<CheckinAsPostDto>> GetCheckinsAsPostsByPlaygroundId(int playgroundId)
        // {
        //     return _mapper.Map<IEnumerable<CheckinAsPostDto>>(await _checkInRepository.Find(
        //         predicate: c => c.PlaygroundId == playgroundId, 
        //         includeProperties: new Expression<Func<CheckIn, object>>[] 
        //                             {
        //                                 (c => c.Member), 
        //                                 (c => c.Member.ProfilePictures)
        //                             },
        //         orderBy: q => q.OrderByDescending(c => c.Created)));
        // }

        // public async Task<IEnumerable<PostDto>> GetPostsByPlaygroundId(int playgroundId)
        // {
        //     List<PostDto> postCollection = new List<PostDto>();

        //     postCollection.AddRange(_mapper.Map<IEnumerable<PostDto>>(await this.GetPhotosAsPostByPlayground(playgroundId)));
        //     var orderedPosts = postCollection.OrderByDescending(x => x.Created);
        //     return orderedPosts;
        // }
    }
}