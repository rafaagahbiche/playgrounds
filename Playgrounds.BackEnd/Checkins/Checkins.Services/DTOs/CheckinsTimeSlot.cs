using System;
using System.Collections.Generic;

namespace Checkins.Services
{
    // A CheckinsTimeSlot represents all checkins during a period of time (2 hours) that starts at StartsAt
    public class Timeslot
    {
        public List<CheckinDto> Checkins { get; set; }
        public DateTime StartsAt { get; set; }

        public int PlaygroundId { get; set; }
        public string PlaygroundAddress { get; set; }
        public Timeslot()
        {
            this.Checkins = new List<CheckinDto>();
        }
    }

    public class PlaygroundTimeslots
    {
        public int PlaygroundId { get; set; }
        public string PlaygroundAddress { get; set; }
        public string PlaygroundPhotoUrl { get; set; }
        public List<Timeslot> Timeslots {get; set; }
        public PlaygroundTimeslots()
        {
            this.Timeslots = new List<Timeslot>();
        }
    }
}