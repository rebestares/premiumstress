using System;
using System.Collections.Generic;

namespace Premiumstress.Core.Domain
{
    public class Post
    {
        public Post()
        {
            PostComments = new List<PostComment>();
        }

        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public int? UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
    }
}