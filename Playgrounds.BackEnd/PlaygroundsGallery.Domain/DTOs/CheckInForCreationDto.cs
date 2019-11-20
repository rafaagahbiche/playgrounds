using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class CheckInForCreationDto
    {
        public DateTime CheckInDate { get; set; }
        public int MemberId { get; set; }
        public int PlaygroundId { get; set; }
    }
}