using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class BlogVideoMap : EntityTypeConfiguration<Core.Domain.Blog.BlogVideo>
    {
        public BlogVideoMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("BlogVideos");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.VideoLink).HasColumnName("VideoLink");
            Property(t => t.BlogID).HasColumnName("BlogID");

            // Relationships
            HasOptional(t => t.Blog)
                .WithMany(t => t.BlogVideos)
                .HasForeignKey(d => d.BlogID);
        }
    }
}