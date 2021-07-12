using GalleryDAL.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalleryDAL.EF.Configurations
{
    public class GalleryImageConfig: IEntityTypeConfiguration<GalleryImage>
    {
        public void Configure(EntityTypeBuilder<GalleryImage> builder)
        {
            builder.HasKey(prop => prop.ID);
        }
    }
}
