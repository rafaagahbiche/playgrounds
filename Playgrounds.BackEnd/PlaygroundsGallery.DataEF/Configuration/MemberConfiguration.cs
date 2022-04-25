namespace PlaygroundsGallery.DataEF.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PlaygroundsGallery.DataEF.Models;
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.Id);
            builder.HasMany(m => m.Photos).WithOne(p => p.Member).HasForeignKey(p => p.MemberId);
            builder.HasAlternateKey(m => m.LoginName);
            builder.HasAlternateKey(m => m.EmailAddress);
            builder.HasMany(m => m.ProfilePictures).WithOne(p => p.Member).HasForeignKey(p => p.MemberId);
        }
    }
}