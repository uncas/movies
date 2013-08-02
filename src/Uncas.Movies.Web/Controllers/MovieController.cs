using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class MovieController : ApiController
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

        // GET api/movie
        public IEnumerable<MovieDto> GetMovies(int rating)
        {
            return Movies.Where(x => x.ImdbRating >= rating)
                         .Select(MapToMovieDto);
        }

        private static MovieDto MapToMovieDto(CinemaShowReadModel x)
        {
            return new MovieDto
                {
                    CinemaUrl = x.CinemaUrl,
                    ImdbRating = x.ImdbRating.ToString("N1"),
                    ImdbUrl = x.ImdbUrl,
                    MovieId = x.MovieId,
                    ShowLocation = x.ShowLocation,
                    ShowTime = GetReadableTime(x.ShowTime),
                    ShowUrl = x.ShowUrl,
                    Title = x.Title
                };
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
                    ImdbRating = (rating ?? 5d + Random.NextDouble()*4d),
                    ShowTime = showTime,
                    ShowLocation = showLocation
                };
        }

        private static string GetReadableTime(DateTime showTime)
        {
            return showTime.ToString("g");
        }
    }
}