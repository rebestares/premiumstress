using System;
using System.Collections.Generic;

namespace Premiumstress.Core.Domain
{
    public class User
    {
        public User()
        {
            Blogs = new List<Blog.Blog>();
            Posts = new List<Post>();
            PostComments = new List<PostComment>();
            Imagelinks = new List<Imagelink>();
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public int ActivityID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? DateJoined { get; set; }
        public bool? IsAdmin { get; set; }
        public string About { get; set; }
        public bool? IsInactive { get; set; }
        public bool? IsLoggedIn { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public virtual ICollection<Blog.Blog> Blogs { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
    }
}