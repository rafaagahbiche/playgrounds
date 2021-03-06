using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class CheckinAsPostDto
    {
        public string AuthorLoginName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}