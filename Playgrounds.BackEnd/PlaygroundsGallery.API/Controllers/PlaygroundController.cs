using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.API.Filters;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Managers;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/playgrounds/")]
    [ApiController]
    public class PlaygroundController: ControllerBase
    {
        private readonly IPlaygroundManager _manager;

        public PlaygroundController(IPlaygroundManager manager)
        {
            this._manager = manager;
        }

        [Route("location/{locationId}")] 
        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundsByLocation(int locationId)
        {
            return Ok(await _manager.GetAllPlaygroundsByLocation(locationId));
        }

        [HttpGet("{playgroundId}")]
        public async Task<IActionResult> GetPlaygroundById(int playgroundId)
        {
            var playground = await _manager.GetPlaygroundById(playgroundId);
            if (playground != null)
            {
                return Ok(playground);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<CheckinDto> GetCheckIn(int checkInId)
        {
            return await _manager.GetCheckInById(checkInId);
        }

        [Authorize]
        [Route("checkin")]
        [HttpPost]
        public async Task<IActionResult> CheckIn(CheckInForCreationDto checkInForCreation)
        {
            if (checkInForCreation == null)
            {
                return BadRequest();
            }
            
            var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            checkInForCreation.MemberId = int.Parse(memberIdStr);
            return StatusCode(201,  await this._manager.CheckInToPlayground(checkInForCreation));
        }

        [HttpGet]
        [Route("{playgroundId}/checkins")]
        public async Task<IActionResult> CheckInsAtPlayground(int playgroundId)
        {
            var playgroundCheckins = await this._manager.GetCheckInsByPlaygroundId(playgroundId);
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