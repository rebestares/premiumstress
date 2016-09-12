using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class ShortFilmMap : EntityTypeConfiguration<Core.Domain.ShortFilm>
    {
        public ShortFilmMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("ShortFilm");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.Content).HasColumnName("Content");
            Property(t => t.DatePosted).HasColumnName("DatePosted");
            Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.CategoryID).HasColumnName("CategoryID");
            Property(t => t.ViewCount).HasColumnName("ViewCount");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.Images).HasColumnName("Images");
            Property(t => t.VideoUrl).HasColumnName("VideoUrl");

            // Relationships
            HasOptional(t => t.Imagelink)
                .WithMany(t => t.ShortFilms)
                .HasForeignKey(d => d.Images);
        }
    }
}