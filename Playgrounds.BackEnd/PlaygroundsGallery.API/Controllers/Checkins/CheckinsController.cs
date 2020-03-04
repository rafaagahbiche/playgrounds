using System;
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
        private readonly ICheckinSchedule _checkinSchedule;

        public CheckinsController(ICheckinManager checkinManager, ICheckinSchedule checkinSchedule)
        {
            this._checkinManager = checkinManager;
            this._checkinSchedule = checkinSchedule;
        }

        [HttpGet]
        [Route("playgrounds/{playgroundId}")]
        public async Task<IActionResult> CheckInsAtPlayground(int playgroundId)
        {
            var playgroundCheckins = await this._checkinManager.GetCheckInsByPlaygroundIdAsync(playgroundId);
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
        [Route("playgrounds/{playgroundId}/{dateTime}")]
        public async Task<IActionResult> ChecksnsAtPlaygroundByDateAsync(int playgroundId, DateTime dateTime)
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