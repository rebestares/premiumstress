using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Premiumstress.Blog.Website.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Fullname
        {
            get { return FirstName + ' ' + LastName; }
            set { if (value == null) throw new ArgumentNullException(nameof(value)); }
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string DateJoined { get; set; }
        public ImageLinkModel ImageLinks { get; set; }
        public string About { get; set; }
        public bool IsLoggedIn { get; set; }
        public string LastLoginDate { get; set; }
        public int BlogPostedCount { get; set; }
        public bool IsAdmin { get; set; }


    }
}