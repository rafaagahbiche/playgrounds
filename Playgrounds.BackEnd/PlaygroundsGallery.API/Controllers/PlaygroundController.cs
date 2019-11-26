using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundsByLocation(int locationId)
        {
            var playgrounds = await _manager.GetAllPlaygroundsByLocation(locationId);
            return Ok(playgrounds);
        }

        [HttpGet("{playgroundId}")]
        public async Task<IActionResult> GetPlaygroundById(int playgroundId)
        {
            var playground = await _manager.GetPlaygroundById(playgroundId);
            return Ok(playground);
        }

        [HttpGet]
        public async Task<CheckInDto> GetCheckIn(int checkInId)
        {
            return await _manager.GetCheckInById(checkInId);
        }

        [Authorize]
        [Route("checkin")]
        [HttpPost]
        public async Task<IActionResult> CheckIn(CheckInForCreationDto checkInForCreation)
        {
            try
            {
                if (checkInForCreation == null)
                {
                    return BadRequest();
                }
                
                var memberIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                checkInForCreation.MemberId = int.Parse(memberIdStr);
                return StatusCode(201,  await this._manager.CheckInToPlayground(checkInForCreation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{playgroundId}/checkins")]
        public async Task<IActionResult> CheckInsAtPlayground(int playgroundId)
        {
            try
            {
                var allCheckIns = await this._manager.GetCheckInsByPlaygroundId(playgroundId);
                if (allCheckIns != null && allCheckIns.Any())
                {
                    return Ok(allCheckIns);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}