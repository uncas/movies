using System.Collections.Generic;
using System.Web.Http;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Controllers
{
    public class MovieController : ApiController
    {
        // GET api/movie
        public IEnumerable<MovieDto> GetMovies()
        {
            return new[]
                {
                    GetMovieDto(1, "Red"),
                    GetMovieDto(2, "Blue"),
                    GetMovieDto(3, "White"),
                };
        }

        private static MovieDto GetMovieDto(int id, string title)
        {
            return new MovieDto
                {
                    MovieId = id,
                    Title = title,
                    ImdbUrl = "http://www.imdb.com/title/tt0111495/",
                    ShowUrl = "http://www.paradisbio.dk",
                    CinemaUrl = "http://www.paradisbio.dk",
                    ImdbRating = "7.3",
                    ShowTime = "I dag 17:30",
                    ShowLocation = "Øst for Paradis, Århus"
                };
        }
    }
}