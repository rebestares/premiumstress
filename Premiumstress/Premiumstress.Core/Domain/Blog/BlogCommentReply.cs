using System;

namespace Premiumstress.Core.Domain.Blog
{
    public class BlogCommentReply
    {
        public int ID { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public int? UserID { get; set; }
        public int? BlogCommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Comment { get; set; }
        public virtual BlogComment BlogComment { get; set; }
    }
}