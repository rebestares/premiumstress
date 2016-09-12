using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class UserMap : EntityTypeConfiguration<Core.Domain.User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("User");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.Birthday).HasColumnName("Birthday");
            Property(t => t.ActivityID).HasColumnName("ActivityID");
            Property(t => t.Password).HasColumnName("Password");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.DateJoined).HasColumnName("DateJoined");
            Property(t => t.IsAdmin).HasColumnName("IsAdmin");
            Property(t => t.About).HasColumnName("About");
            Property(t => t.IsInactive).HasColumnName("IsInactive");
            Property(t => t.IsLoggedIn).HasColumnName("IsLoggedIn");
            Property(t => t.LastLoggedIn).HasColumnName("LastLoggedIn");
        }
    }
}