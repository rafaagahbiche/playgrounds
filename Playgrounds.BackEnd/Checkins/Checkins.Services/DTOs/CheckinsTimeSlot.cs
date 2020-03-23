using System;
using System.Collections.Generic;

namespace Checkins.Services
{
    // A CheckinsTimeSlot represents all checkins during a period of time (2 hours) that starts at StartsAt
    public class CheckinsTimeSlot
    {
        public List<CheckinDto> Checkins { get; set; }
        public DateTime StartsAt { get; set; }

        public CheckinsTimeSlot()
        {
            this.Checkins = new List<CheckinDto>();
        }
    }
}