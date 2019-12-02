using System.Collections.Generic;

namespace PlaygroundsGallery.Domain.Models
{
	public class Member : Entity
    {
		public string LoginName { get; set; }
		public string EmailAddress { get; set; }
		public byte[] PasswordSalt { get; set; }
		public byte[] PasswordHash { get; set; }
		public ICollection<Photo> Photos { get; set; }
		public ICollection<ProfilePicture> ProfilePictures { get; set; }
        public ICollection<CheckIn> CheckIns { get; set; }
	}
}
