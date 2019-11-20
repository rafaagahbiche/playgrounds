using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class PhotoDto
    {
				public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    		public DateTime Created { get; set; }

        public string PublicId { get; set; }
				public int MemberId { get; set; }

        public int PlaygroundId { get; set; }
    }
}
