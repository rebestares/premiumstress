using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class BlogCommentMap : EntityTypeConfiguration<Core.Domain.Blog.BlogComment>
    {
        public BlogCommentMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("BlogComment");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.DatePosted).HasColumnName("DatePosted");
            Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.BlogID).HasColumnName("BlogID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.Website).HasColumnName("Website");
            Property(t => t.Comment).HasColumnName("Comment");

            // Relationships
            HasOptional(t => t.Blog)
                .WithMany(t => t.BlogComments)
                .HasForeignKey(d => d.BlogID);
        }
    }
}