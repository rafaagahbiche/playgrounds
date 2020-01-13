using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.API.Filters;
using PlaygroundsGallery.Domain.Managers;

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
            if (photo != null)
            {
                return Ok(photo);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("recent")]
        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetRecentPhotos(int count)
        {
            var recentPhotos = await _frontManager.GetRecentPhotos(count);
            if (recentPhotos != null && recentPhotos.Any())
            {
                return Ok(recentPhotos);
            }
            else
            {
                return NoContent();
            }
        }

        [Route("playground/{playgroundId}")]
        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundPhotos(int playgroundId)
        {
            var playgroundPhotos = await _frontManager.GetPhotosByPlayground(playgroundId);
            if (playgroundPhotos != null && playgroundPhotos.Any())
            {
                return Ok(playgroundPhotos);
            }
            else
            {
                return NoContent();
            }
        }

        [Route("post/playground/{playgroundId}")]
        [ClientCacheControlFilter(ClientCacheControl.Public, 120)]
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundPhotosAsPosts(int playgroundId)
        {
            var playgroundPhotosPosts = await _frontManager.GetPhotosAsPostByPlayground(playgroundId);
            if (playgroundPhotosPosts != null && playgroundPhotosPosts.Any())
            {
                return Ok(playgroundPhotosPosts);
            }
            else
            {
                return NoContent();
            }
        }
    }
}