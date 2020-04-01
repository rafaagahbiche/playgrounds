using System;

namespace Photos.Services.DTOs
{
    public class PhotoDto: PhotoToUpdateDto
    {
        public string PlaygroundAddress { get; set; }
        public int PlaygroundLocationId { get; set; }
        public string PlaygroundLocationStr { get; set; }
        public string MemberLoginName { get; set; }
    }
}
