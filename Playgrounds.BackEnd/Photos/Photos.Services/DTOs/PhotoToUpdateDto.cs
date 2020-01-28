using System;

namespace Photos.Services.DTOs
{
    public class PhotoToUpdateDto: PhotoToInsertDto
    {
        public int Id { get; set; }
    	public DateTime Created { get; set; }
    }
}