using System;

namespace Premiumstress.Core.Domain.Blog
{
    public partial class BlogCategory
    {
        public int ID { get; set; }
        public Nullable<int> BlogID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Category Category { get; set; }
    }
}
