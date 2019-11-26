using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoToUpdateDto: PhotoToInsertDto
    {
        public int Id { get; set; }
    	public DateTime Created { get; set; }
        public int? PlaygroundId { get; set; }
    }
}