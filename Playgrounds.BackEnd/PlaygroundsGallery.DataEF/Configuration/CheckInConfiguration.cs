using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaygroundsGallery.Domain.Models;

namespace PlaygroundsGallery.DataEF.Configuration
{
    public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(c => c.Playground).WithMany(p => p.CheckIns).HasForeignKey(c => c.PlaygroundId);
            builder.HasOne(c => c.Member).WithMany(m => m.CheckIns).HasForeignKey(c => c.MemberId);
        }
    }
}