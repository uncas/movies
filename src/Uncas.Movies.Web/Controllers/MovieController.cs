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
        public IEnumerable<MovieDto> GetMovies(int rating)
        {
            var cinemaShowReadStore = new FakeCinemaShowReadStore();
            return cinemaShowReadStore.GetMovieShows(rating)
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

        private static string GetReadableTime(DateTime showTime)
        {
            return showTime.ToString("g");
        }
    }
}