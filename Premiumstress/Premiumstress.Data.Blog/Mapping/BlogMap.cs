using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class BlogMap : EntityTypeConfiguration<Core.Domain.Blog.Blog>
    {
        public BlogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Blog");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.DatePosted).HasColumnName("DatePosted");
            this.Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            this.Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");
            this.Property(t => t.ViewCount).HasColumnName("ViewCount");
            this.Property(t => t.IsPromoted).HasColumnName("IsPromoted");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.IsSuggested).HasColumnName("IsSuggested");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasMany(t => t.Imagelinks)
                .WithMany(t => t.Blogs)
                .Map(m =>
                    {
                        m.ToTable("BlogImage");
                        m.MapLeftKey("BlogID");
                        m.MapRightKey("ImageID");
                    });

            this.HasMany(t => t.Keywords)
                .WithMany(t => t.Blogs)
                .Map(m =>
                    {
                        m.ToTable("BlogKeywords");
                        m.MapLeftKey("BlogID");
                        m.MapRightKey("KeywordID");
                    });

            this.HasOptional(t => t.User)
                .WithMany(t => t.Blogs)
                .HasForeignKey(d => d.UserID);

        }
    }
}
