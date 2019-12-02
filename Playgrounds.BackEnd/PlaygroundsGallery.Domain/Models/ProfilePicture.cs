namespace PlaygroundsGallery.Domain.Models
{
    public class ProfilePicture: Entity
    {
		public string Url { get; set; }
		public string PublicId { get; set; }
		public int MemberId { get; set; }
		public virtual Member Member { get; set; }
		public bool Deleted { get; set; } = false;
		public bool Main { get; set; } = true;
    }
}