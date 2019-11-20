using System.Collections.Generic;

namespace PlaygroundsGallery.Domain.Models
{
	public class Member : Entity
    {
		public string LoginName { get; set; }
		public string EmailAddress { get; set; }
		public byte[] PasswordSalt { get; set; }
		public byte[] PasswordHash { get; set; }
		public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
	}
}
