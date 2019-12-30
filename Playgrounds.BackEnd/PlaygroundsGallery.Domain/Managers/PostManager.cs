using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Repositories;

namespace PlaygroundsGallery.Domain.Managers
{
    public class PostManager : IPostManager
    {
        private readonly IRepository<CheckIn> _checkInRepository;
        private readonly IRepository<Photo> _photoRepository;
        private readonly IMapper _mapper;

        public PostManager(
            IRepository<CheckIn> checkInRepository,
            IRepository<Photo> photoRepository,
            IMapper mapper)
        {
            this._checkInRepository = checkInRepository;
            this._photoRepository = photoRepository;
            this._mapper = mapper;
        }

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

        public async Task<IEnumerable<CheckinAsPostDto>> GetCheckinsAsPostsByPlaygroundId(int playgroundId)
        {
            return _mapper.Map<IEnumerable<CheckinAsPostDto>>(await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId, 
                includeProperties: new Expression<Func<CheckIn, object>>[] 
                                    {
                                        (c => c.Member), 
                                        (c => c.Member.ProfilePictures)
                                    },
                orderBy: q => q.OrderByDescending(c => c.Created)));
        }

        public async Task<IEnumerable<PostDto>> GetPostsByPlaygroundId(int playgroundId)
        {
            List<PostDto> postCollection = new List<PostDto>();

            postCollection.AddRange(_mapper.Map<IEnumerable<PostDto>>(await this.GetPhotosAsPostByPlayground(playgroundId)));
            postCollection.AddRange(_mapper.Map<IEnumerable<PostDto>>(await this.GetCheckinsAsPostsByPlaygroundId(playgroundId)));
            var orderedPosts = postCollection.OrderByDescending(x => x.Created);
            return orderedPosts;
        }
    }
}