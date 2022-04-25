using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Services
{
    public class CheckinsOrganizer
    {
        private readonly IMapper _mapper;
        public CheckinsOrganizer(IMapper mapper)
        {
            this._mapper = mapper;
        }
        
        public List<PlaygroundTimeslots> OrganizeCheckinsIntoPlaygroundTimeslots(IEnumerable<CheckIn> checkins)
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
                    Timeslots = CheckinsToTimeslotsForSinglePlayground(checkinsByPlayground)
                });
            }

            return playgrounds;
        }

        public List<Timeslot> CheckinsToTimeslotsForMultiplePlaygrounds(IEnumerable<CheckIn> checkins)
        {
            var timeslots = new List<Timeslot>();
            var playgroundIds = checkins.Select(c => c.PlaygroundId).Distinct();
            foreach (var playgroundId in playgroundIds)
            {
                var checkinsByPlayground = checkins.Where(c => c.PlaygroundId == playgroundId);
                timeslots.AddRange(CheckinsToTimeslotsForSinglePlayground(checkinsByPlayground));
            }

            return timeslots;
        }

        public List<Timeslot> CheckinsToTimeslotsForSinglePlayground(IEnumerable<CheckIn> checkins)
        {
            var timeslots = new List<Timeslot>();
            if (checkins != null && checkins.Any())
            {
                var singleTimeslot = new Timeslot();
                foreach (var checkin in checkins)
                {
                    if (timeslots.Any() && checkin.CheckInDate <= singleTimeslot?.StartsAt.AddHours(2))
                    {
                        singleTimeslot.Checkins.Add(_mapper.Map<CheckinDto>(checkin));
                    }
                    else
                    {
                        singleTimeslot = this.GetNewCheckinsTimeSlot(checkin);
                        timeslots.Add(singleTimeslot);
                    }
                }
            }

            return timeslots;
        }
        
        private Timeslot GetNewCheckinsTimeSlot(CheckIn checkin)
        {
            var newTimeSlot = new Timeslot();
            newTimeSlot.Checkins.Add(_mapper.Map<CheckinDto>(checkin));
            newTimeSlot.StartsAt = checkin.CheckInDate;
            newTimeSlot.PlaygroundId = checkin.PlaygroundId;
            newTimeSlot.PlaygroundAddress = checkin.Playground?.Address;
            return newTimeSlot;
        }
    }
}