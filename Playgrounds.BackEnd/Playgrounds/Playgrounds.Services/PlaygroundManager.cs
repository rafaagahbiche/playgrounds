using System;
using System.Threading.Tasks;
using AutoMapper;
using Entity = PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Playgrounds.Services
{
    public class PlaygroundManager : IPlaygroundManager
    {
        private readonly IRepository<Entity.Playground> _playgroundRepository;
		private readonly IRepository<Entity.Location> _locationRepository;
		private readonly IMapper _mapper;

        public PlaygroundManager(
            IRepository<Entity.Playground> playgroundRepository,
            IRepository<Entity.Location> locationRepository,
            IMapper mapper)
        {
            this._playgroundRepository = playgroundRepository;
            this._locationRepository = locationRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocations()
            => _mapper.Map<IEnumerable<LocationDto>>(await _locationRepository.GetAll());
        

        public async Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocation(int locationId)
        {
            var playgrounds = await _playgroundRepository
                    .Find(predicate: p => p.LocationId == locationId, 
                          includeProperties: new Expression<Func<Entity.Playground, object>>[]{ (p => p.Location), (p => p.Photos) });
            
            return _mapper.Map<IEnumerable<PlaygroundDto>>(playgrounds);
        }

        public async Task<PlaygroundDto> GetPlaygroundById(int playgroundId) 
        {
            var playground = await _playgroundRepository.SingleOrDefault(
                                        predicate: p => p.Id == playgroundId, 
                                        includeProperties: new Expression<Func<Entity.Playground, object>>[]
                                                { (p => p.Location), (p => p.Photos) });
            return _mapper.Map<PlaygroundDto>(playground);
        }
    }
}