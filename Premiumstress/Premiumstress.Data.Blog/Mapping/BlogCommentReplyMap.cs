using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class BlogCommentReplyMap : EntityTypeConfiguration<Core.Domain.Blog.BlogCommentReply>
    {
        public BlogCommentReplyMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("BlogCommentReply");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.DatePosted).HasColumnName("DatePosted");
            Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.BlogCommentID).HasColumnName("BlogCommentID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.Website).HasColumnName("Website");
            Property(t => t.Comment).HasColumnName("Comment");

            // Relationships
            HasOptional(t => t.BlogComment)
                .WithMany(t => t.BlogCommentReplies)
                .HasForeignKey(d => d.BlogCommentID);
        }
    }
}