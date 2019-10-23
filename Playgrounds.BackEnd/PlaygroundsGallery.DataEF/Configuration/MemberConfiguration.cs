namespace PlaygroundsGallery.DataEF.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PlaygroundsGallery.Domain.Models;
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasMany(m => m.Photos).WithOne(p => p.Member).HasForeignKey(p => p.MemberId);
        }
    }
}