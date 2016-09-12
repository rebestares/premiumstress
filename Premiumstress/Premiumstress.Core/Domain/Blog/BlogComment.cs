using System;
using System.Collections.Generic;

namespace Premiumstress.Core.Domain.Blog
{
    public class BlogComment
    {
        public BlogComment()
        {
            BlogCommentReplies = new List<BlogCommentReply>();
        }

        public int ID { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public int? UserID { get; set; }
        public int? BlogID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Comment { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<BlogCommentReply> BlogCommentReplies { get; set; }
    }
}