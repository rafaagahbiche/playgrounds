using System;
using Microsoft.AspNetCore.Http;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoForCreationDto
    {
				public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    		public DateTime UploadDate { get; set; }
        public string PublicId { get; set; }
				public int MemberId { get; set; }
        public IFormFile File { get; set; }
    }
}
