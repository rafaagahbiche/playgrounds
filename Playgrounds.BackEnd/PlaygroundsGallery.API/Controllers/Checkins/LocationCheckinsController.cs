using System;
using System.Linq;
using System.Threading.Tasks;
using Checkins.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaygroundsGallery.API.Controllers.Checkins
{
    [Route("api/checkins/")]
    public class LocationCheckinsController: ControllerBase
    {
        private readonly ILocationCheckinService _checkinSchedule;

        public LocationCheckinsController(ILocationCheckinService checkinSchedule)
        {
            this._checkinSchedule = checkinSchedule;
        }

        [HttpGet]
        [Route("locations/{locationId}/timeslots/{dateTime}")]
        public async Task<IActionResult> CheckinsAtLocationByDateAsync(int locationId, DateTime dateTime)
        {
            var locationCheckins = await this._checkinSchedule.GetPlaygroundTimeslotsAtLocationByDateAsync(locationId, dateTime);
            if (locationCheckins != null && locationCheckins.Any())
            {
                return Ok(locationCheckins);
            }
            else
            {
                return NoContent();
            }
        }
    }
}