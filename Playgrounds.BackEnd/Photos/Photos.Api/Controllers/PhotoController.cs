using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Photo.Api.Filters;
using Photos.Services.Managers;

namespace Photo.Api.Controllers
{
    [Route("api/photos/")]
    [ApiController]
    public class PhotoController: ControllerBase
    {
        private readonly IPhotoManager _photoManager;

        public PhotoController(IPhotoManager photoManager)
        {
            _photoManager = photoManager;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _photoManager.GetPhoto(id);
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
            var recentPhotos = await _photoManager.GetRecentPhotos(count);
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
            var playgroundPhotos = await _photoManager.GetPhotosByPlayground(playgroundId);
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
            var playgroundPhotosPosts = await _photoManager.GetPhotosAsPostByPlayground(playgroundId);
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