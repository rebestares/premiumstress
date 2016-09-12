using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Premiumstress.Blog.Website.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Type { get; set; }
        public int? DisplayOrder { get; set; }
    }
}