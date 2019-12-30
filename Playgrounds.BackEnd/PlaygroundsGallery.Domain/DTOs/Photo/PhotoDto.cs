using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoDto: PhotoToUpdateDto
    {
        public string PlaygroundAddress { get; set; }
        public int PlaygroundLocationId { get; set; }
        public string PlaygroundLocationStr { get; set; }
    }
}
