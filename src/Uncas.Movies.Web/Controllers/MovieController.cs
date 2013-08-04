using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class MovieController : ApiController
    {
        // GET api/movie
        public IEnumerable<MovieDto> GetMovies(int rating, int day)
        {
            var cinemaShowReadStore = new CinemaShowReadStore();
            return cinemaShowReadStore.GetMovieShows(rating, day)
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
                    ShowLocation = "Øst for Paradis, Århus",
                    ShowTime = GetReadableTime(x.ShowTime),
                    ShowUrl = x.ShowUrl,
                    Title = x.Title,
                    MovieUrl = x.MovieUrl
                };
        }

        private static string GetReadableTime(DateTime showTime)
        {
            return showTime.ToString("g");
        }
    }
}