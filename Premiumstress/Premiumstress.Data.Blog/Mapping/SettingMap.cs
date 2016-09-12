using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class SettingMap : EntityTypeConfiguration<Core.Domain.Setting>
    {
        public SettingMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Settings");
            Property(t => t.ID).HasColumnName("ID");
        }
    }
}