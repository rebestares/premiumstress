using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class PostCommentMap : EntityTypeConfiguration<Core.Domain.PostComment>
    {
        public PostCommentMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("PostComment");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.DatePosted).HasColumnName("DatePosted");
            Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.Content).HasColumnName("Content");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.PostID).HasColumnName("PostID");

            // Relationships
            HasOptional(t => t.Post)
                .WithMany(t => t.PostComments)
                .HasForeignKey(d => d.PostID);
            HasOptional(t => t.User)
                .WithMany(t => t.PostComments)
                .HasForeignKey(d => d.UserID);
        }
    }
}