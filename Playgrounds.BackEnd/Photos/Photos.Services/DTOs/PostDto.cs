using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PostDto
    {
        public string AuthorLoginName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public DateTime Created { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}