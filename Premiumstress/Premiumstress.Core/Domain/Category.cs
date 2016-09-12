using System;
using System.Collections.Generic;
using Premiumstress.Core.Domain.Blog;

namespace Premiumstress.Core.Domain
{
    public partial class Category
    {
        public Category()
        {
            this.BlogCategories = new List<BlogCategory>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BlogCategory> BlogCategories { get; set; }
        public  Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> ParentCategoryID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Type { get; set; }
    }
}
