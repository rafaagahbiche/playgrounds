using System;

namespace PlaygroundsGallery.Domain.Models
{
	public class Photo: Entity
    {
		public string Url { get; set; }
		public string PublicId { get; set; }
		public DateTime UploadDate { get; set; }
		public string Description { get; set; }
		public int MemberId { get; set; }
		public bool Deleted { get; set; } = false;
		public virtual Member Member { get; set; }
	}
}
