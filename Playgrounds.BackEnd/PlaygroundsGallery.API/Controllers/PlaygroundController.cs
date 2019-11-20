using System;
using System.Threading.Tasks;
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

        // [HttpGet]
        // public async Task<IActionResult> GetLocations()
        // {
        //     var locations = await _manager.GetAllLocations();
        //     return Ok(locations);
        // }

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

                return StatusCode(201,  await this._manager.CheckInToPlayground(checkInForCreation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}