using Microsoft.EntityFrameworkCore;
using PlaygroundsGallery.DataEF.Configuration;
using PlaygroundsGallery.Domain.Models;
using System.Threading.Tasks;

namespace PlaygroundsGallery.DataEF
{
    public class GalleryContext: DbContext, IGalleryContext
	{
		public GalleryContext(DbContextOptions<GalleryContext> options) 
			: base(options)
		{
		}

		public DbSet<Member> Members { get; set; }
		public DbSet<Photo> Photos { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

		public void CurrentDatabaseMigrate()
		{
			base.Database.Migrate();
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new MemberConfiguration());
			modelBuilder.ApplyConfiguration(new PhotoConfiguration());
		}
    }
}
