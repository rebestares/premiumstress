using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class PostMap : EntityTypeConfiguration<Core.Domain.Post>
    {
        public PostMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("Post");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Content).HasColumnName("Content");
            Property(t => t.DatePosted).HasColumnName("DatePosted");
            Property(t => t.DateDeleted).HasColumnName("DateDeleted");
            Property(t => t.DateUpdated).HasColumnName("DateUpdated");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            HasOptional(t => t.User)
                .WithMany(t => t.Posts)
                .HasForeignKey(d => d.UserID);
        }
    }
}