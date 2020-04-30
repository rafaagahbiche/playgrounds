using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playgrounds.Services;
using PlaygroundsGallery.API.Filters;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/locations/")]
    [ApiController]
    public class LocationsController: ControllerBase
    {
        private readonly IPlaygroundManager _playgroundManager;

        public LocationsController(IPlaygroundManager manager)
        {
            this._playgroundManager = manager;
        }

        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetAllLocation(int locationId)
        {
            return Ok(await _playgroundManager.GetAllLocationsAsync());
        }
    }
}