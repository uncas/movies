using System;

namespace Uncas.Movies.Web.Models
{
    public class CinemaShowReadModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string ImdbUrl { get; set; }
        public string ShowUrl { get; set; }
        public string CinemaUrl { get; set; }
        public double ImdbRating { get; set; }
        public DateTime ShowTime { get; set; }
        public string ShowLocation { get; set; }
    }
}