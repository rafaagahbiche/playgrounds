namespace PlaygroundsGallery.Domain.Models
{
    public class Photo: Entity
    {
		public string Url { get; set; }
		public string PublicId { get; set; }
		public string Description { get; set; }
		public int MemberId { get; set; }
		public virtual Member Member { get; set; }
		public int? PlaygroundId { get; set; }
		public virtual Playground Playground { get; set; }
		public bool Deleted { get; set; } = false;
	}
}
