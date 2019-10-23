using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Repositories;

namespace PlaygroundsGallery.DataEF.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(IGalleryContext dbContext) : base(dbContext)
        {

        }
    }
}