using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkins.Services
{
    public interface ICheckinManager
    {
        Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundIdAsync(int playgroundId);
        Task<IEnumerable<CheckinDto>> GetCheckinsByPlaygroundIdByDateAsync(int playgroundId, DateTime dateTime);
    }
}