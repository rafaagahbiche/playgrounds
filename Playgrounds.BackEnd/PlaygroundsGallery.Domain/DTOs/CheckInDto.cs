using System;

namespace PlaygroundsGallery.Domain.DTOs
{
    public class CheckInDto
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public string MemberLoginName { get; set; }
        public string MemberProfilePictureUrl { get; set; }
        public string PlaygroundAddress { get; set; }
    }
}