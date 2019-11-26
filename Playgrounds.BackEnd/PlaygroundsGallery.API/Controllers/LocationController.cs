using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.Domain.Managers;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/locations/")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IPlaygroundManager _manager;

        public LocationController(IPlaygroundManager manager)
        {
            this._manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _manager.GetAllLocations();
            return Ok(locations);
        }
    }
}