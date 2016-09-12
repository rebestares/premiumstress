using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Premiumstress.Blog.Website.Models
{
    public class BlogModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<string> Keywords { get; set; }
        public string Content { get; set; }
        public string DatePosted { get; set; }
        public UserModel User { get; set; }
        public bool IsOwner { get; set; }
        public CategoryModel Category { get; set; }
        public ImageLinkModel ImageLinks { get; set; }
        public int UserID { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsSuggested { get; set; }
        public string  ViewCount { get; set; }
        public bool? IsApproved { get; set; }
   //   public List<CommentClass> Comments { get; set; } not implemented
        public string VideoLink { get; set; }

        public void SanitizeContent()
        {
           Content = Regex.Replace(Content, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);

            //Sanitize content  for blog home
           Content = Content.Length > 200
                ? Content.Substring(0, 200)
                : Content;
        }
    }
}