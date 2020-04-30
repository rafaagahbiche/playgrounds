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
    public class PlaygroundCheckinService : IPlaygroundCheckinService
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<PlaygroundCheckinService> _logger;
        
        public PlaygroundCheckinService(
            GalleryContext context,
            IMapper mapper,
            ILogger<PlaygroundCheckinService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkinsDtos = Enumerable.Empty<CheckinDto>();
            try
            {
                var checkinsEntities = await _context.CheckIns
                        .Include(ch => ch.Member)
                        .ThenInclude(m => m.ProfilePictures)
                        .Where(c => c.PlaygroundId == playgroundId 
                                    && c.CheckInDate.Day == dateTime.Day
                                    && c.CheckInDate.Month == dateTime.Month
                                    && c.CheckInDate.Year == dateTime.Year)
                        .OrderBy(ch => ch.CheckInDate)
                        .ToListAsync();

                checkinsDtos =  _mapper.Map<IEnumerable<CheckinDto>>(checkinsEntities);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, $"Exception while searching for checkins by playground id: {playgroundId} and date : {dateTime}.");
            }

            return checkinsDtos;
        }

        public async Task<IEnumerable<Timeslot>> GetTimeSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtPlaygroundByDateAsync(playgroundId, dateTime);
            return OrganizeCheckinsIntoTimeslots(checkins);
        }

        private List<Timeslot> OrganizeCheckinsIntoTimeslots(IEnumerable<CheckinDto> checkins)
        {
            List<Timeslot> timeSlots = new List<Timeslot>();
            if (checkins != null && checkins.Any())
            {
                if (checkins.Count() == 1)
                {
                    timeSlots.Add(new Timeslot(){
                        Checkins = checkins.ToList(),
                        StartsAt = checkins.FirstOrDefault().CheckInDate
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
                            singleTimeSlot.Checkins.Add(checkin);
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

        private Timeslot GetNewCheckinsTimeSlot(CheckinDto checkinDto)
        {
            var newTimeSlot = new Timeslot();
            newTimeSlot.Checkins.Add(checkinDto);
            newTimeSlot.StartsAt = checkinDto.CheckInDate;
            return newTimeSlot;
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