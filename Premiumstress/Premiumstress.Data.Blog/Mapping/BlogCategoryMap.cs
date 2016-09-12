using System.Data.Entity.ModelConfiguration;
using Premiumstress.Core.Domain.Blog;

namespace Premiumstress.Data.Blog.Mapping
{
    public class BlogCategoryMap : EntityTypeConfiguration<BlogCategory>
    {
        public BlogCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlogCategory");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BlogID).HasColumnName("BlogID");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");

            // Relationships
            this.HasOptional(t => t.Blog)
                .WithMany(t => t.BlogCategories)
                .HasForeignKey(d => d.BlogID);
            this.HasOptional(t => t.Category)
                .WithMany(t => t.BlogCategories)
                .HasForeignKey(d => d.CategoryID);

        }
    }
}
