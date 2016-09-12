using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class PostComment
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Content { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> PostID { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
