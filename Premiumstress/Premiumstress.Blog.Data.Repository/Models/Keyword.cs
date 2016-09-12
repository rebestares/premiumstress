using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class Keyword
    {
        public Keyword()
        {
            this.Blogs = new List<Blog>();
        }

        public int ID { get; set; }
        public string Keywords { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
