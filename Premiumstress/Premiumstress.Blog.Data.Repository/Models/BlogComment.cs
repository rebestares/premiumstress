using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class BlogComment
    {
        public BlogComment()
        {
            this.BlogCommentReplies = new List<BlogCommentReply>();
        }

        public int ID { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> BlogID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Comment { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<BlogCommentReply> BlogCommentReplies { get; set; }
    }
}
