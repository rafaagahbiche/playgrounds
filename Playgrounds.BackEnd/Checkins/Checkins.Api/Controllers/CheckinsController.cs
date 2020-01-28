using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Checkins.Services;

namespace Checkins.Api.Controllers
{
    [Route("api/checkins/")]
    [ApiController]
    public class CheckinsController: ControllerBase
    {
        private readonly ICheckinManager _checkinManager;
        public CheckinsController(ICheckinManager checkinManager)
        {
            this._checkinManager = checkinManager;
        }

        [HttpGet]
        public async Task<CheckinDto> GetCheckin(int checkInId)
        {
            return await _checkinManager.GetCheckInById(checkInId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkin(CheckInForCreationDto checkInForCreation)
        {
            if (checkInForCreation == null)
            {
                return BadRequest();
            }
            
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            checkInForCreation.MemberId = int.Parse(memberIdStr);
            return StatusCode(201,  await this._checkinManager.CheckInToPlayground(checkInForCreation));
        }

        [HttpGet]
        [Route("{playgroundId}")]
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