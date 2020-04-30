using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkins.Services
{
    public interface IMemberCheckinService
    {
        Task<CheckinDto> GetCheckInById(int checkInId);
        Task<CheckinDto> CheckinToPlaygroundAsync(CheckInForCreationDto checkInForCreation);
        Task<bool> CancelCheckinToPlaygroundAsync(int checkinId);
       
    }
}