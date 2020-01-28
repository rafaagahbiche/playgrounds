using System.Collections.Generic;

namespace PlaygroundsGallery.DataEF.Models
{
    public class Playground : Entity
    {
        public Location Location { get; set; }
        public int? LocationId { get; set; }
        public string Address { get; set; }
        // public virtual Photo MainPhoto { get; set; }
        // public int MainPhotoId { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
    }
}