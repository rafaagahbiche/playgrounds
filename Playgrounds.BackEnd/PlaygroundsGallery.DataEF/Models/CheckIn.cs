using System;

namespace PlaygroundsGallery.DataEF.Models
{
    // A Checkin is the presence of a member in a specific Playground at a certain time
    public class CheckIn : Entity
    {
        public Playground Playground { get; set; }
        public int PlaygroundId { get; set; }
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}