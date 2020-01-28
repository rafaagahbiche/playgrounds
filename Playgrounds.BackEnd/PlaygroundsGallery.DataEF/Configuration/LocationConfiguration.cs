using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.Id);
            builder.HasMany(l => l.Playgrounds).WithOne(p => p.Location).HasForeignKey(p => p.LocationId);
        }
    }
}