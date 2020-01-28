using System;

namespace Photos.Services.DTOs
{
    public class PhotoToInsertDto
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string PublicId { get; set; }
        public int? PlaygroundId { get; set; }
		public int MemberId { get; set; }
    }
}
