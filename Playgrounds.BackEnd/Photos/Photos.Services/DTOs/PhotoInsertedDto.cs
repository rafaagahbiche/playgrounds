using System;

namespace Photos.Services.DTOs
{
    public class PhotoInsertedDto: PhotoToInsertDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}