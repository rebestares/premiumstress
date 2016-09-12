using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class Category
    {
        public Category()
        {
            this.Blogs = new List<Blog>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
