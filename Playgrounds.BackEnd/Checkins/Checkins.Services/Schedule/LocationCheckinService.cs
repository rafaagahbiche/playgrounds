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

        public LocationCheckinService(
            GalleryContext context,
            IMapper mapper,
            ILogger<LocationCheckinService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<IEnumerable<PlaygroundTimeslots>> GetPlaygroundTimeslotsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtLocationByDateAsync(locationId, dateTime);
            return OrganizeCheckinsIntoPlaygroundTimeslots(checkins);
        }
        
        public async Task<IEnumerable<Timeslot>> GetTimeSlotsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtLocationByDateAsync(locationId, dateTime);
            return OrganizeCheckinsIntoTimeslots(checkins);
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

        private List<PlaygroundTimeslots> OrganizeCheckinsIntoPlaygroundTimeslots(IEnumerable<CheckIn> checkins)
        {
            var playgrounds = new List<PlaygroundTimeslots>();
            var playgroundsCheckins = checkins.Select(ch => new 
                {
                    ch.PlaygroundId, 
                    ch.Playground.Address, 
                    ch.Playground.Photos?.FirstOrDefault()?.Url
                }).Distinct();

            foreach (var playgroundCheckins in playgroundsCheckins)
            {
                var checkinsByPlayground = checkins.Where(ch => ch.PlaygroundId == playgroundCheckins.PlaygroundId);
                playgrounds.Add(new PlaygroundTimeslots()
                {
                    PlaygroundId = playgroundCheckins.PlaygroundId,
                    PlaygroundAddress = playgroundCheckins.Address,
                    PlaygroundPhotoUrl = playgroundCheckins.Url,
                    Timeslots = OrganizeCheckinsIntoTimeslots(checkinsByPlayground)
                });
            }

            return playgrounds;
        }

        private List<Timeslot> OrganizeCheckinsIntoTimeslots(IEnumerable<CheckIn> checkins)
        {
            var timeSlots = new List<Timeslot>();
            if (checkins != null && checkins.Any())
            {
                if (checkins.Count() == 1)
                {
                    var checkin = checkins.FirstOrDefault();
                    timeSlots.Add(new Timeslot(){
                        StartsAt = checkin.CheckInDate,
                        Checkins = _mapper.Map<List<CheckinDto>>(checkins),
                    });
                }
                else
                {
                    var singleTimeSlot = new Timeslot()
                    {
                        StartsAt = DateTime.MinValue
                    };
                    foreach (var checkin in checkins)
                    {
                        if (checkin.CheckInDate <= singleTimeSlot.StartsAt.AddHours(2))
                        {
                            singleTimeSlot.Checkins.Add(_mapper.Map<CheckinDto>(checkin));
                        }
                        else
                        {
                            singleTimeSlot = this.GetNewCheckinsTimeSlot(checkin);
                            timeSlots.Add(singleTimeSlot);
                        }
                    }
                }
            }

            return timeSlots;
        }

        
        private Timeslot GetNewCheckinsTimeSlot(CheckIn checkin)
        {
            var newTimeSlot = new Timeslot();
            newTimeSlot.Checkins.Add(_mapper.Map<CheckinDto>(checkin));
            newTimeSlot.StartsAt = checkin.CheckInDate;
            return newTimeSlot;
        }
    }
}