using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Configuration
{
    public class ProfilePictureConfiguration: IEntityTypeConfiguration<ProfilePicture>
    {
        public void Configure(EntityTypeBuilder<ProfilePicture> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Member).WithMany(m => m.ProfilePictures).HasForeignKey(p => p.MemberId);
        }
    }
}