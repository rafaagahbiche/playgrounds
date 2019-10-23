using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Repositories;

namespace PlaygroundsGallery.DataEF.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(IGalleryContext dbContext) : base(dbContext)
        {
        }
    }
}