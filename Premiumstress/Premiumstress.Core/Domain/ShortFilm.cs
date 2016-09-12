using System;
using System.Collections.Generic;

namespace Premiumstress.Core.Domain
{
    public class ShortFilm
    {
        public ShortFilm()
        {
            Imagelinks = new List<Imagelink>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CategoryID { get; set; }
        public int? ViewCount { get; set; }
        public int? UserID { get; set; }
        public int? Images { get; set; }
        public string VideoUrl { get; set; }
        public virtual Imagelink Imagelink { get; set; }
        public virtual ShortFilmCategory ShortFilmCategory { get; set; }
        public virtual ICollection<Imagelink> Imagelinks { get; set; }
    }
}