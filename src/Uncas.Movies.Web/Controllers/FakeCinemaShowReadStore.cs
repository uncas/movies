using System;
using System.Collections.Generic;
using System.Linq;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class FakeCinemaShowReadStore
    {
        private static readonly Random Random = new Random(1);
        private static IList<CinemaShowReadModel> _movies;

        private static IEnumerable<CinemaShowReadModel> Movies
        {
            get
            {
                if (_movies == null)
                {
                    _movies = new List<CinemaShowReadModel>();
                    for (int i = 0; i < 100; i++)
                        _movies.Add(GetCinemaShow(i + 1, "Red", DateTime.Now.AddHours(i)));
                }

                return _movies;
            }
        }

        public IEnumerable<CinemaShowReadModel> GetMovieShows(
            int? minimumRating,
            int day)
        {
            return Movies.Where(
                x =>
                x.ImdbRating >= minimumRating.GetValueOrDefault(5) &&
                x.ShowTime >= DateTime.Now &&
                x.ShowTime.Date == DateTime.Now.AddDays(day).Date);
        }

        private static CinemaShowReadModel GetCinemaShow(
            int id,
            string title,
            DateTime showTime,
            double? rating = null,
            string showLocation = "Øst for Paradis, Århus")
        {
            return new CinemaShowReadModel
                {
                    MovieId = id,
                    Title = title,
                    ImdbUrl = "http://www.imdb.com/title/tt0111495/",
                    ShowUrl = "http://www.paradisbio.dk",
                    CinemaUrl = "http://www.paradisbio.dk",
                    ImdbRating = (rating ?? 5d + Random.NextDouble()*4.5d),
                    ShowTime = showTime,
                    ShowLocation = showLocation
                };
        }
    }
}