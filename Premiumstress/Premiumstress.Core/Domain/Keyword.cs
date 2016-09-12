using System.Collections.Generic;

namespace Premiumstress.Core.Domain
{
    public class Keyword
    {
        public Keyword()
        {
            Blogs = new List<Blog.Blog>();
        }

        public int ID { get; set; }
        public string Keywords { get; set; }
        public virtual ICollection<Blog.Blog> Blogs { get; set; }
    }
}