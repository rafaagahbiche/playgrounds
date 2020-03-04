using System;
using System.Collections.Generic;

namespace Checkins.Services
{
    public class CheckinsTimeSlot
    {
        public ICollection<CheckinDto> Checkins { get; set; }
        public DateTime StartsAt { get; set; }

        public CheckinsTimeSlot()
        {
            this.Checkins = new List<CheckinDto>();
        }
    }
}