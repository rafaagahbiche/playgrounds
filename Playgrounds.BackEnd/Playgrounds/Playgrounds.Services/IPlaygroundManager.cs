using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playgrounds.Services
{
    public interface IPlaygroundManager
    {
        Task<IEnumerable<LocationDto>> GetAllLocations();
        Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocation(int locationId);
        Task<PlaygroundDto> GetPlaygroundById(int playgroundId);
        Task<PlaygroundDto> GetPlaygroundByAddress(string playgroundAddress);
    }
}