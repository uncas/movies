using System;

namespace Uncas.Movies.Web.Models
{
    public class Movie
    {
        public double ImdbRating { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Director { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Rated { get; set; }

        // Unused:
        public string Poster { get; set; }

        public TimeSpan GetRuntimeAsTimeSpan()
        {
            string[] parts = Runtime.Split(' ');
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[2]);
            return TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));
        }
    }
}