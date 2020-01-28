using System.Linq;
using System.Threading.Tasks;
using Checkins.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/checkins/")]
    public class CheckinsController: ControllerBase
    {
        private readonly ICheckinManager _checkinManager;

        public CheckinsController(ICheckinManager checkinManager)
        {
            this._checkinManager = checkinManager;
        }

        [HttpGet]
        [Route("playgrounds/{playgroundId}")]
        public async Task<IActionResult> CheckInsAtPlayground(int playgroundId)
        {
            var playgroundCheckins = await this._checkinManager.GetCheckInsByPlaygroundId(playgroundId);
            if (playgroundCheckins != null && playgroundCheckins.Any())
            {
                return Ok(playgroundCheckins);
            }
            else
            {
                return NoContent();
            }
        }
    }
}