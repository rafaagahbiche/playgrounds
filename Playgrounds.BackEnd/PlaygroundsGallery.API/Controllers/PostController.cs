using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlaygroundsGallery.Domain.Managers;

namespace PlaygroundsGallery.API.Controllers
{
    [Route("api/posts/")]
    [ApiController]

    public class PostController: ControllerBase
    {
        private readonly IPostManager _manager;

        public PostController(IPostManager manager)
        {
            this._manager = manager;
        }

        [Route("playground/{playgroundId}")]
        [HttpGet]
        public async Task<IActionResult> GetPlaygroundPhotosAsPosts(int playgroundId)
        {
            var posts = await _manager.GetPostsByPlaygroundId(playgroundId);
            if (posts != null && posts.Any())
            {
                return Ok(posts);
            }
            else
            {
                return StatusCode(204);
            }
        }
    }
}