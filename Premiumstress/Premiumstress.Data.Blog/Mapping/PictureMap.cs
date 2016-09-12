using System.Data.Entity.ModelConfiguration;

namespace Premiumstress.Data.Blog.Mapping
{
    public class PictureMap : EntityTypeConfiguration<Core.Domain.Picture>
    {
        public PictureMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Pictures");
            Property(t => t.ID).HasColumnName("ID");
        }
    }
}