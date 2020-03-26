using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Checkins.Services
{
    public class CheckinSchedule : ICheckinSchedule
    {
        private readonly IRepository<CheckIn> _checkInRepository;
		private readonly IMapper _mapper;
        public CheckinSchedule(
            IRepository<CheckIn> checkInRepository,
            IMapper mapper)
        {
            this._checkInRepository = checkInRepository;
            this._mapper = mapper;
        }
        
        public async Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            return _mapper.Map<IEnumerable<CheckinDto>>(await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId 
                                && c.CheckInDate.Day == dateTime.Day
                                && c.CheckInDate.Month == dateTime.Month
                                && c.CheckInDate.Year == dateTime.Year, 
                includeProperties: new Expression<Func<CheckIn, object>>[] 
                                    {
                                        (c => c.Member), 
                                        (c => c.Member.ProfilePictures), 
                                        (c => c.Playground)
                                    },
                orderBy: q => q.OrderBy(c => c.CheckInDate)));
        }

        public async Task<IEnumerable<CheckinsTimeSlot>> GetTimeSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkins = await GetCheckinsAtPlaygroundByDateAsync(playgroundId, dateTime);
            // checkins = checkins.OrderBy(c => c.CheckInDate);
            List<CheckinsTimeSlot> timeSlots = new List<CheckinsTimeSlot>();
            if (checkins != null && checkins.Any())
            {
                if (checkins.Count() == 1)
                {
                    timeSlots.Add(new CheckinsTimeSlot(){
                        Checkins = checkins.ToList(),
                        StartsAt = checkins.FirstOrDefault().CheckInDate
                    });
                }
                else
                {
                    CheckinsTimeSlot singleTimeSlot = new CheckinsTimeSlot()
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

        private CheckinsTimeSlot GetNewCheckinsTimeSlot(CheckinDto checkinDto)
        {
            var newTimeSlot = new CheckinsTimeSlot();
            newTimeSlot.Checkins.Add(checkinDto);
            newTimeSlot.StartsAt = checkinDto.CheckInDate;
            return newTimeSlot;
        }

        public async Task<IEnumerable<TimeSpan>> GetCheckinsSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var checkinsByDate = await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId 
                                && c.CheckInDate.Day == dateTime.Day
                                && c.CheckInDate.Month == dateTime.Month
                                && c.CheckInDate.Year == dateTime.Year, 
                orderBy: q => q.OrderByDescending(c => c.CheckInDate));
            var timeSpans = checkinsByDate.Select(x => x.CheckInDate.TimeOfDay).Distinct();
            // foreach (var item in timeSpans)
            // {
            //     item.
            // }
            return timeSpans;
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundBetweenTwoDatesAsync(int playgroundId, DateTime startDateTime, DateTime endDateTime)
        {
            // IEnumerable<CheckinDto> checkins = Enumerable.Empty<CheckinDto>();
            // if (startDateTime.Year == endDateTime.Year 
            //     && startDateTime.Month == endDateTime.Month
            //     && startDateTime.Day == endDateTime.Day)
            // {
                return _mapper.Map<IEnumerable<CheckinDto>>(await _checkInRepository.Find(
                    predicate: c => c.PlaygroundId == playgroundId 
                                    && c.CheckInDate >= startDateTime
                                    && c.CheckInDate <= endDateTime, 
                    includeProperties: new Expression<Func<CheckIn, object>>[] 
                                        {
                                            (c => c.Member), 
                                            (c => c.Member.ProfilePictures), 
                                            (c => c.Playground)
                                        },
                    orderBy: q => q.OrderByDescending(c => c.CheckInDate)));
            // }
            // else
            // {
            //     return Enumerable.Empty<CheckinDto>();
            // }
        }
    }
}