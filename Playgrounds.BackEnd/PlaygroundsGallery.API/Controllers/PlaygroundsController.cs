using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.API.Filters;
using Playgrounds.Services;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/playgrounds/")]
    [ApiController]
    public class PlaygroundsController: ControllerBase
    {
        private readonly IPlaygroundManager _playgroundManager;

        public PlaygroundsController(IPlaygroundManager manager)
        {
            this._playgroundManager = manager;
        }

        [Route("locations/{locationId}")] 
        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundsByLocation(int locationId)
        {
            return Ok(await _playgroundManager.GetAllPlaygroundsByLocation(locationId));
        }

        [HttpGet("{playgroundId}")]
        public async Task<IActionResult> GetPlaygroundById(int playgroundId)
        {
            var playground = await _playgroundManager.GetPlaygroundById(playgroundId);
            if (playground != null)
            {
                return Ok(playground);
            }
            else
            {
                return NoContent();
            }
        }
    }
}