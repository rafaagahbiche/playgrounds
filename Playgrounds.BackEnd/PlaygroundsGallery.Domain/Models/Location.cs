using System.Collections.Generic;

namespace PlaygroundsGallery.Domain.Models
{
    public class Location: Entity
    {
        public string Country { get; set; }
        public string City { get; set; }
        public virtual ICollection<Playground> Playgrounds { get; set; }
    }
}