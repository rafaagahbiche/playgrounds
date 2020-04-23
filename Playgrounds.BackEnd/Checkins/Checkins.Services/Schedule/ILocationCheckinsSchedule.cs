using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Services
{
    public interface ILocationCheckinsSchedule
    {
        Task<IEnumerable<Timeslot>> GetTimeSlotsAtLocationByDateAsync(int locationId, DateTime dateTime);
        Task<IEnumerable<PlaygroundTimeslots>> GetPlaygroundTimeslotsAtLocationByDateAsync(int locationId, DateTime dateTime);
    }
}