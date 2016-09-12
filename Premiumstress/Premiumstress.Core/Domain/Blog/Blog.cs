using System;
using System.Collections.Generic;

namespace Premiumstress.Core.Domain.Blog
{
    public partial class Blog
    {
        public Blog()
        {
            this.BlogCategories = new List<BlogCategory>();
            this.BlogComments = new List<BlogComment>();
            this.BlogVideos = new List<BlogVideo>();
            this.Imagelinks = new List<Imagelink>();
            this.Keywords = new List<Keyword>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPromoted { get; set; }
        public bool? IsSuggested { get; set; }
        public int? CategoryID { get; set; }
        public int? ViewCount { get; set; }
        public int? UserID { get; set; }
        public bool? IsApproved { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BlogCategory> BlogCategories { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<BlogVideo> BlogVideos { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
    }
}
