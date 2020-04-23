using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using PlaygroundsGallery.DataEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Playgrounds.Services
{
    public class PlaygroundManager : IPlaygroundManager
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PlaygroundManager> _logger;

        public PlaygroundManager(
            GalleryContext context,
            IMapper mapper,
            ILogger<PlaygroundManager> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocations()
            => _mapper.Map<IEnumerable<LocationDto>>(await _context.Playgrounds.ToListAsync());
        

        public async Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocation(int locationId)
        {
            var playgrounds = Enumerable.Empty<PlaygroundDto>();
            try 
            {
                var playgroundEntities = await _context.Playgrounds
                                    .Include(p => p.Location)
                                    .Include(p => p.Photos)
                                    .Where(p => p.LocationId == locationId)
                                    .ToListAsync();
                playgrounds = _mapper.Map<IEnumerable<PlaygroundDto>>(playgroundEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while searching for playgrounds by location id.");
            }
            
            return playgrounds;
        }

        public async Task<PlaygroundDto> GetPlaygroundById(int playgroundId) 
        {
            PlaygroundDto playground = null;
            try 
            {
                var playgroundEntity = await _context.Playgrounds
                                    .Include(p => p.Location)
                                    .Include(p => p.Photos)
                                    .SingleOrDefaultAsync(p => p.Id == playgroundId);
                playground = _mapper.Map<PlaygroundDto>(playgroundEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while searching for playgrounds by id.");
            }

            return playground;
        }

        public async Task<PlaygroundDto> GetPlaygroundByAddress(string playgroundAddress) 
        {
            PlaygroundDto playground = null;
            try 
            {
                var playgroundEntity = await _context.Playgrounds
                                .Include(p => p.Location)
                                .Include(p => p.Photos)
                                .SingleOrDefaultAsync(p => p.Address.Equals(playgroundAddress, StringComparison.CurrentCultureIgnoreCase)); 
                                             
                playground = _mapper.Map<PlaygroundDto>(playgroundEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while searching for playgrounds by id.");
            }

            return playground;
        }
    }
}