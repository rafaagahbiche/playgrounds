using System.Collections.Generic;
using System.Threading.Tasks;

namespace Checkins.Services
{
    public interface ICheckinManager
    {
         Task<CheckinDto> CheckInToPlayground(CheckInForCreationDto checkInForCreation);
         Task<CheckinDto> GetCheckInById(int checkInId);
         Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundId(int playgroundId);
    }
}