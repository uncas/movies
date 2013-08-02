using System;
using System.Collections.Generic;
using System.Web.Http;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class MovieController : ApiController
    {
        private static readonly Random Random = new Random(1);
        private static IList<MovieDto> _movies;

        private static IEnumerable<MovieDto> Movies
        {
            get
            {
                if (_movies == null)
                {
                    _movies = new List<MovieDto>();
                    for (int i = 0; i < 100; i++)
                        _movies.Add(GetMovieDto(i + 1, "Red", DateTime.Now.AddHours(i)));
                }

                return _movies;
            }
        }

        // GET api/movie
        public IEnumerable<MovieDto> GetMovies()
        {
            return Movies;
        }

        private static MovieDto GetMovieDto(
            int id,
            string title,
            DateTime showTime,
            double? rating = null,
            string showLocation = "Øst for Paradis, Århus")
        {
            return new MovieDto
                {
                    MovieId = id,
                    Title = title,
                    ImdbUrl = "http://www.imdb.com/title/tt0111495/",
                    ShowUrl = "http://www.paradisbio.dk",
                    CinemaUrl = "http://www.paradisbio.dk",
                    ImdbRating = (rating ?? 5d + Random.NextDouble()*4d).ToString("N1"),
                    ShowTime = GetReadableTime(showTime),
                    ShowLocation = showLocation
                };
        }

        private static string GetReadableTime(DateTime showTime)
        {
            return showTime.ToString("g");
        }
    }
}