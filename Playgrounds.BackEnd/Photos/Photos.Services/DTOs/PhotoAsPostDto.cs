using System;

namespace Photos.Services.DTOs
{
    public class PhotoAsPostDto
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string AuthorLoginName { get; set; }
        public string AuthorProfilePictureUrl { get; set; }
        public DateTime Created { get; set; }
    }
}