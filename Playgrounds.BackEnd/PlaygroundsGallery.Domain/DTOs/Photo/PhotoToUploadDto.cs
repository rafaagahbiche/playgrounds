using Microsoft.AspNetCore.Http;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoToUploadDto
    {
        public string Description { get; set; }
				public int MemberId { get; set; }
        public int? PlaygroundId { get; set; }
        public IFormFile File { get; set; }
    }
}
