namespace Premiumstress.Core.Domain
{
    public class ShortFilmCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ShortFilm ShortFilm { get; set; }
    }
}