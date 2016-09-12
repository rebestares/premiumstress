using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class BlogVideo
    {
        public int ID { get; set; }
        public string VideoLink { get; set; }
        public Nullable<int> BlogID { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
