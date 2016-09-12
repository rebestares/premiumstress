using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class ShortFilmCategoryMap : EntityTypeConfiguration<Core.Domain.ShortFilmCategory>
    {
        public ShortFilmCategoryMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("ShortFilmCategory");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");

            // Relationships
            HasRequired(t => t.ShortFilm)
                .WithOptional(t => t.ShortFilmCategory);
        }
    }
}