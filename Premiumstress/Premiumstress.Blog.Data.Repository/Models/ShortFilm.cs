using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class ShortFilm
    {
        public ShortFilm()
        {
            this.Imagelinks = new List<Imagelink>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Images { get; set; }
        public string VideoUrl { get; set; }
        public virtual Imagelink Imagelink { get; set; }
        public virtual ShortFilmCategory ShortFilmCategory { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
    }
}
