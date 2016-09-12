using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class LocationMap : EntityTypeConfiguration<Core.Domain.Location>
    {
        public LocationMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Location");
            Property(t => t.ID).HasColumnName("ID");
        }
    }
}