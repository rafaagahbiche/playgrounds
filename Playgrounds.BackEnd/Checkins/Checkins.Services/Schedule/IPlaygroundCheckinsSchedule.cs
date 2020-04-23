using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkins.Services
{
    public interface IPlaygroundCheckinsSchedule
    {
        Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime);
        Task<IEnumerable<CheckinDto>> GetCheckinsAtPlaygroundBetweenTwoDatesAsync(int playgroundId, DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<Timeslot>> GetTimeSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime);
    }
}