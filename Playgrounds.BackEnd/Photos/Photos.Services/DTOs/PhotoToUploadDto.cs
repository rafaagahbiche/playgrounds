using Microsoft.AspNetCore.Http;

namespace Photos.Services.DTOs
{
    public class PhotoToUploadDto
    {
        public string Description { get; set; }
				public int MemberId { get; set; }
        public int? PlaygroundId { get; set; }
        public IFormFile File { get; set; }
    }
}
