using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Checkins.Services;
using System;

namespace Checkins.Api.Controllers
{
    [Route("api/checkins/")]
    [ApiController]
    public class CheckinsController: ControllerBase
    {
        private readonly ICheckinManager _checkinManager;
        private readonly ICheckinMember _checkinMember;
        private readonly IPlaygroundCheckinsSchedule _checkinSchedule;
        public CheckinsController(
            ICheckinManager checkinManager, 
            ICheckinMember checkinMember,
            IPlaygroundCheckinsSchedule checkinSchedule)
        {
            this._checkinManager = checkinManager;
            this._checkinMember = checkinMember;
            this._checkinSchedule = checkinSchedule;
        }

        [HttpGet]
        public async Task<CheckinDto> GetCheckin(int checkInId)
        {
            return await _checkinMember.GetCheckInById(checkInId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckinAsync(CheckInForCreationDto checkInForCreation)
        {
            if (checkInForCreation == null)
            {
                return BadRequest();
            }
            
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            checkInForCreation.MemberId = int.Parse(memberIdStr);
            return StatusCode(201,  await this._checkinMember.CheckinToPlaygroundAsync(checkInForCreation));
        }

        [Authorize]
        [HttpDelete]
        [Route("{checkinId}")]
        public async Task<IActionResult> CancelCheckinAsync(int checkinId)
        {
            var deletionSucceeded = await this._checkinMember.CancelCheckinToPlaygroundAsync(checkinId);
            if (deletionSucceeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("playgrounds/{playgroundId}/{dateTime}")]
        public async Task<IActionResult> CheckinsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var playgroundCheckins = await this._checkinSchedule.GetCheckinsAtPlaygroundByDateAsync(playgroundId, dateTime);
            if (playgroundCheckins != null && playgroundCheckins.Any())
            {
                return Ok(playgroundCheckins);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("playgrounds/{playgroundId}/slots/{dateTime}")]
        public async Task<IActionResult> CheckinsSlotsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
        {
            var timeSlots = await this._checkinSchedule.GetTimeSlotsAtPlaygroundByDateAsync(playgroundId, dateTime);
            if (timeSlots != null && timeSlots.Any())
            {
                return Ok(timeSlots);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("playgrounds/{playgroundId}/{startDateTime}/{endDateTime}")]
        // /api/checkins/playgrounds/4/2020-02-04T16:00:00/2020-02-04T17:00:00
        public async Task<IActionResult> CheckinsAtPlaygroundBetweenDatesAsync(int playgroundId, DateTime startDateTime, DateTime endDateTime)
        {
            var playgroundCheckins = await this._checkinSchedule.GetCheckinsAtPlaygroundBetweenTwoDatesAsync(playgroundId, startDateTime, endDateTime);
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