namespace Uncas.Movies.Web.Models
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImdbUrl { get; set; }
        public string ShowUrl { get; set; }
        public string CinemaUrl { get; set; }
        public string ImdbRating { get; set; }
        public string ShowTime { get; set; }
        public string ShowLocation { get; set; }
        public string MovieUrl { get; set; }
    }
}