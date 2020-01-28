using System;

namespace PlaygroundsGallery.DataEF.Models
{
    public class CheckIn : Entity
    {
        public Playground Playground { get; set; }
        public int PlaygroundId { get; set; }
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public DateTime CheckInDate { get; set; }
    }
}