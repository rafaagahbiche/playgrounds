using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.Domain.Managers;
using PlaygroundsGallery.Helper.Exceptions;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/photos/")]
    [ApiController]
    public class PhotoController: ControllerBase
    {

        private readonly IFrontManager _frontManager;

        public PhotoController(IFrontManager frontManager)
        {
            _frontManager = frontManager;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _frontManager.GetPhoto(id);
            return Ok(photo);
        }

        [Route("recent")]
        [HttpGet]
        public async Task<IActionResult> GetRecentPhotos(int count)
        {
            var photos = await _frontManager.GetRecentPhotos(count);
            return Ok(photos);
        }
    }
}