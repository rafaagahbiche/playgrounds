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
         Task<CheckInDto> CheckInToPlayground(CheckInForCreationDto checkInForCreation);
         Task<CheckInDto> GetCheckInById(int checkInId);
         Task<PlaygroundDto> GetPlaygroundById(int playgroundId);
    }
}