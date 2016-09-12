using System.Collections.Generic;

namespace Premiumstress.Core.Domain
{
    public class Imagelink
    {
        public Imagelink()
        {
            ShortFilms = new List<ShortFilm>();
            Blogs = new List<Blog.Blog>();
            ShortFilms1 = new List<ShortFilm>();
            Users = new List<User>();
        }

        public int ID { get; set; }
        public string FullImageLink { get; set; }
        public string ThumbnailImageLink { get; set; }
        public string MediumImageLink { get; set; }
        public virtual ICollection<ShortFilm> ShortFilms { get; set; }
        public virtual ICollection<Blog.Blog> Blogs { get; set; }
        public virtual ICollection<ShortFilm> ShortFilms1 { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}