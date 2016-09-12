using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class Blog
    {
        public Blog()
        {
            this.BlogComments = new List<BlogComment>();
            this.BlogVideos = new List<BlogVideo>();
            this.Imagelinks = new List<Imagelink>();
            this.Keywords = new List<Keyword>();
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
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<BlogVideo> BlogVideos { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}
