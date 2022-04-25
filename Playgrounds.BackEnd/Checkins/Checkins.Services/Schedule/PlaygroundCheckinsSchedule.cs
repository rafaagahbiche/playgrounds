using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Services
{
    public class PlaygroundCheckinService : IPlaygroundCheckinService
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PlaygroundCheckinService> _logger;
        private readonly CheckinsOrganizer _organizer;
        
        public PlaygroundCheckinService(
            GalleryContext context,
            IMapper mapper,
            ILogger<PlaygroundCheckinService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
            this._organizer = new CheckinsOrganizer(_mapper);
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkins = await GetCheckinEntitiesAtPlaygroundByDateAsync(playgroundId, dateTime);
            return _mapper.Map<IEnumerable<CheckinDto>>(checkins);
        }

        public async Task<IEnumerable<Timeslot>> GetTimeSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkins = await GetCheckinEntitiesAtPlaygroundByDateAsync(playgroundId, dateTime);
            return _organizer.CheckinsToTimeslotsForSinglePlayground(checkins);
        }

        private async Task<IEnumerable<CheckIn>> GetCheckinEntitiesAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkinsEntities = Enumerable.Empty<CheckIn>();
            try
            {
                checkinsEntities = await _context.CheckIns
                        .Include(ch => ch.Member)
                        .ThenInclude(m => m.ProfilePictures)
                        .Where(c => c.PlaygroundId == playgroundId 
                                    && c.CheckInDate.Day == dateTime.Day
                                    && c.CheckInDate.Month == dateTime.Month
                                    && c.CheckInDate.Year == dateTime.Year)
                        .OrderBy(ch => ch.CheckInDate)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, $"Exception while searching for checkins by playground id: {playgroundId} and date : {dateTime}.");
            }

            return checkinsEntities;
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundBetweenTwoDatesAsync(int playgroundId, DateTime startDateTime, DateTime endDateTime)
        {
            var checkinsDtos = Enumerable.Empty<CheckinDto>();
            try
            {
                var checkinsEntities = await _context.CheckIns
                                    .Include(ch => ch.Playground)
                                    .Include(ch => ch.Member)
                                    .ThenInclude(m => m.ProfilePictures)
                                    .Where(c => c.PlaygroundId == playgroundId 
                                        && c.CheckInDate >= startDateTime
                                        && c.CheckInDate <= endDateTime)
                                    .OrderByDescending(ch => ch.CheckInDate)
                                    .ToListAsync();

                checkinsDtos =  _mapper.Map<IEnumerable<CheckinDto>>(checkinsEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception while searching for checkins by playground id: {playgroundId} between date1 : {startDateTime} and date2: {endDateTime}.");
            }
            return checkinsDtos;
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