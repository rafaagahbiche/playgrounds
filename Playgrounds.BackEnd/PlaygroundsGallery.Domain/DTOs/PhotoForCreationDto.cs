using System;
using Microsoft.AspNetCore.Http;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoForCreationDto
    {
        public string Description { get; set; }
				public int MemberId { get; set; }
        public IFormFile File { get; set; }
    }
}
