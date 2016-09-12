using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class PlaceMap : EntityTypeConfiguration<Core.Domain.Place>
    {
        public PlaceMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Title)
                .IsRequired();

            // Table & Column Mappings
            ToTable("Place");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Title).HasColumnName("Title");
        }
    }
}