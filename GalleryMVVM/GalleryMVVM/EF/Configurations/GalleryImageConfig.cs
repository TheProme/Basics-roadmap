using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryMVVM.EF.Configurations
{
    public class GalleryImageConfig: IEntityTypeConfiguration<GalleryImage>
    {
        public void Configure(EntityTypeBuilder<GalleryImage> builder)
        {
            builder.HasKey(prop => prop.ID);
            builder.Ignore(prop => prop.IsChangable);
        }
    }
}
