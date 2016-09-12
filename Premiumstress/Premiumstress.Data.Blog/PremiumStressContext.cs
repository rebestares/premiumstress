using System.Data.Entity;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog.Mapping;

namespace Premiumstress.Data.Blog
{
    using Core.Domain.Blog;

    public partial class PremiumStressContext : DbContext
    {
        static PremiumStressContext()
        {
            Database.SetInitializer<PremiumStressContext>(null);
        }

        public PremiumStressContext()
            : base("Name=PremiumStressContext")
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogCommentReply> BlogCommentReplies { get; set; }
        public DbSet<BlogVideo> BlogVideos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Imagelink> Imagelinks { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ShortFilm> ShortFilms { get; set; }
        public DbSet<ShortFilmCategory> ShortFilmCategories { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new BlogCategoryMap());
            modelBuilder.Configurations.Add(new BlogCommentMap());
            modelBuilder.Configurations.Add(new BlogCommentReplyMap());
            modelBuilder.Configurations.Add(new BlogVideoMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new FoodMap());
            modelBuilder.Configurations.Add(new ImagelinkMap());
            modelBuilder.Configurations.Add(new KeywordMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new PictureMap());
            modelBuilder.Configurations.Add(new PlaceMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new PostCommentMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new ShortFilmMap());
            modelBuilder.Configurations.Add(new ShortFilmCategoryMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VideoMap());
        }
    }
}
