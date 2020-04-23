using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;

namespace Checkins.Services
{
    public class CheckinManager : ICheckinManager
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<CheckinManager> _logger;        
        
        public CheckinManager(
            GalleryContext context,
            IMapper mapper,
            ILogger<CheckinManager> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundIdAsync(int playgroundId)
        {
            var checkinsDtos = Enumerable.Empty<CheckinDto>();
            try
            {
                var checkinsEntities =  await _context.CheckIns
                            .Include(ch => ch.Playground)
                            .Include(ch => ch.Member)
                            .ThenInclude(m => m.ProfilePictures)
                            .OrderByDescending(ch => ch.CheckInDate)
                            .ToListAsync();

                checkinsDtos = _mapper.Map<IEnumerable<CheckinDto>>(checkinsEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while searching for checkins by playground id: {playgroundId}.");
            }

            return checkinsDtos;
        }
    }
}