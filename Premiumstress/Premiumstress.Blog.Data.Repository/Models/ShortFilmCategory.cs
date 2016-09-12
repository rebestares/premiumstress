using System;
using System.Collections.Generic;

namespace Premiumstress.Data.Blog.Models
{
    public partial class ShortFilmCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ShortFilm ShortFilm { get; set; }
    }
}
