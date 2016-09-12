using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class KeywordMap : EntityTypeConfiguration<Core.Domain.Keyword>
    {
        public KeywordMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Keyword");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Keywords).HasColumnName("Keywords");
        }
    }
}