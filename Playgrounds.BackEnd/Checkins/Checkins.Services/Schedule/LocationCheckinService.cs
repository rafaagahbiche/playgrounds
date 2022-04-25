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
    public class LocationCheckinService : ILocationCheckinService
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<LocationCheckinService> _logger;

        private readonly CheckinsOrganizer _organizer;

        public LocationCheckinService(
            GalleryContext context,
            IMapper mapper,
            ILogger<LocationCheckinService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
            this._organizer = new CheckinsOrganizer(_mapper);
        }

        public async Task<IEnumerable<PlaygroundTimeslots>> GetPlaygroundTimeslotsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtLocationByDateAsync(locationId, dateTime);
            return _organizer.OrganizeCheckinsIntoPlaygroundTimeslots(checkins);
        }
        
        public async Task<IEnumerable<Timeslot>> GetTimeSlotsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtLocationAfterDateAsync(locationId, dateTime);
            var timeslots = _organizer.CheckinsToTimeslotsForMultiplePlaygrounds(checkins);
            return timeslots.OrderBy(t => t.StartsAt);
        }
        
        private async Task<IEnumerable<CheckIn>> GetCheckinsAtLocationAfterDateAsync(int locationId, DateTime dateTime)
        {
            var checkinsEntities = Enumerable.Empty<CheckIn>();
            try
            {
                checkinsEntities = await _context.CheckIns
                        .Include(ch => ch.Playground)
                        .Include(ch => ch.Member)
                        .ThenInclude(m => m.ProfilePictures)
                        .Where(c => c.Playground.LocationId == locationId 
                                    && c.CheckInDate.Day >= dateTime.Day
                                    && c.CheckInDate.Month >= dateTime.Month
                                    && c.CheckInDate.Year >= dateTime.Year)
                        .OrderBy(ch => ch.PlaygroundId)
                        .ThenBy(ch => ch.CheckInDate)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception while searching for checkins by location id: {locationId} and date : {dateTime}.");
            }

            return checkinsEntities;
        }

        private async Task<IEnumerable<CheckIn>> GetCheckinsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var checkinsEntities = Enumerable.Empty<CheckIn>();
            try
            {
                checkinsEntities = await _context.CheckIns
                        .Include(ch => ch.Playground)
                        .ThenInclude(p => p.Photos)
                        .Include(ch => ch.Member)
                        .ThenInclude(m => m.ProfilePictures)
                        .Where(c => c.Playground.LocationId == locationId 
                                    && c.CheckInDate.Day == dateTime.Day
                                    && c.CheckInDate.Month == dateTime.Month
                                    && c.CheckInDate.Year == dateTime.Year)
                        .OrderBy(ch => ch.PlaygroundId)
                        .ThenBy(ch => ch.CheckInDate)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception while searching for checkins by location id: {locationId} and date : {dateTime}.");
            }

            return checkinsEntities;
        }
    }
}