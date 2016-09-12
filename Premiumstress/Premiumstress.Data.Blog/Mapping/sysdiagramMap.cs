using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class sysdiagramMap : EntityTypeConfiguration<Core.Domain.sysdiagram>
    {
        public sysdiagramMap()
        {
            // Primary Key
            HasKey(t => t.diagram_id);

            // Properties
            Property(t => t.name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            ToTable("sysdiagrams");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.principal_id).HasColumnName("principal_id");
            Property(t => t.diagram_id).HasColumnName("diagram_id");
            Property(t => t.version).HasColumnName("version");
            Property(t => t.definition).HasColumnName("definition");
        }
    }
}