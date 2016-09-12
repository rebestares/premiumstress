using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class Imagelink
    {
        public Imagelink()
        {
            this.ShortFilms = new List<ShortFilm>();
            this.Blogs = new List<Blog>();
            this.ShortFilms1 = new List<ShortFilm>();
            this.Users = new List<User>();
        }

        public int ID { get; set; }
        public string FullImageLink { get; set; }
        public string ThumbnailImageLink { get; set; }
        public string MediumImageLink { get; set; }
        public virtual ICollection<ShortFilm> ShortFilms { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<ShortFilm> ShortFilms1 { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
