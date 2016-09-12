using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class User
    {
        public User()
        {
            this.Blogs = new List<Blog>();
            this.Posts = new List<Post>();
            this.PostComments = new List<PostComment>();
            this.Imagelinks = new List<Imagelink>();
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public int ActivityID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateJoined { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public string About { get; set; }
        public Nullable<bool> IsInactive { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<System.DateTime> LastLoggedIn { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
    }
}
