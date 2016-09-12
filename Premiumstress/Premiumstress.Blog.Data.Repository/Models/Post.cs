using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class Post
    {
        public Post()
        {
            this.PostComments = new List<PostComment>();
        }

        public int ID { get; set; }
        public string Content { get; set; }
        public System.DateTime DatePosted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
    }
}
