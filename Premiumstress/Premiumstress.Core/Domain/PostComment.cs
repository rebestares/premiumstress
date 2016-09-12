using System;

namespace Premiumstress.Core.Domain
{
    public class PostComment
    {
        public int ID { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public string Content { get; set; }
        public int? UserID { get; set; }
        public int? PostID { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}