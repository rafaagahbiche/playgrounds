using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoInsertedDto: PhotoToInsertDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}