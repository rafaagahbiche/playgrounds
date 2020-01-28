using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Configuration
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Member).WithMany(m => m.Photos).HasForeignKey(p => p.MemberId);
            builder.HasOne(p => p.Playground).WithMany(p => p.Photos).HasForeignKey(p => p.PlaygroundId);
        }
    }
}