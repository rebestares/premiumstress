using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class FoodMap : EntityTypeConfiguration<Core.Domain.Food>
    {
        public FoodMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Title)
                .IsRequired();

            // Table & Column Mappings
            ToTable("Food");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Title).HasColumnName("Title");
        }
    }
}