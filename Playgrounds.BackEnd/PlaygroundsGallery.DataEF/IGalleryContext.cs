using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PlaygroundsGallery.DataEF
{
    public interface IGalleryContext
    {
		int SaveChanges();
		Task<int> SaveChangesAsync();
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void CurrentDatabaseMigrate();
    }
}
