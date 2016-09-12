using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class ImagelinkMap : EntityTypeConfiguration<Core.Domain.Imagelink>
    {
        public ImagelinkMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Imagelinks");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.FullImageLink).HasColumnName("FullImageLink");
            Property(t => t.ThumbnailImageLink).HasColumnName("ThumbnailImageLink");
            Property(t => t.MediumImageLink).HasColumnName("MediumImageLink");

            // Relationships
            HasMany(t => t.ShortFilms1)
                .WithMany(t => t.Imagelinks)
                .Map(m =>
                {
                    m.ToTable("ShortFilmImage");
                    m.MapLeftKey("ImageID");
                    m.MapRightKey("ShortFilmID");
                });

            HasMany(t => t.Users)
                .WithMany(t => t.Imagelinks)
                .Map(m =>
                {
                    m.ToTable("UserImage");
                    m.MapLeftKey("ImageID");
                    m.MapRightKey("UserID");
                });
        }
    }
}