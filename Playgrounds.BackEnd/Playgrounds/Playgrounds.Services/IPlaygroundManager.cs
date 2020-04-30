using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playgrounds.Services
{
    public interface IPlaygroundManager
    {
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();
        Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocationIdAsync(int locationId);
        Task<PlaygroundDto> GetPlaygroundByIdAsync(int playgroundId);
        Task<PlaygroundDto> GetPlaygroundByAddressAsync(string playgroundAddress);
    }
}