using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.DTOs;

namespace PlaygroundsGallery.Domain.Managers
{
    public interface IPlaygroundManager
    {
         Task<IEnumerable<LocationDto>> GetAllLocations();
         Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocation(int locationId);
         Task<CheckinDto> CheckInToPlayground(CheckInForCreationDto checkInForCreation);
         Task<CheckinDto> GetCheckInById(int checkInId);
         Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundId(int playgroundId);
         Task<PlaygroundDto> GetPlaygroundById(int playgroundId);
    }
}