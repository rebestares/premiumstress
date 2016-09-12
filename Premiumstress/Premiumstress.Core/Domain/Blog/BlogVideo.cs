namespace Premiumstress.Core.Domain.Blog
{
    public class BlogVideo
    {
        public int ID { get; set; }
        public string VideoLink { get; set; }
        public int? BlogID { get; set; }
        public virtual Blog Blog { get; set; }
    }
}