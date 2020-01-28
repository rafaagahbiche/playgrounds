using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Configuration
{
    public class PlaygroundConfiguration : IEntityTypeConfiguration<Playground>
    {
        public void Configure(EntityTypeBuilder<Playground> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.Photos).WithOne(p => p.Playground).HasForeignKey(p => p.PlaygroundId);
            builder.HasOne(p => p.Location).WithMany(l => l.Playgrounds).HasForeignKey(p => p.LocationId);
        }
    }
}