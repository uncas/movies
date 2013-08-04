using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Uncas.Movies.Web.Jobs;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Web.Crawling
{
    public class CinemaCrawler
    {
        private readonly CinemaShowReadStore _cinemaShowReadStore =
            new CinemaShowReadStore();

        private readonly CrawledMovieRepository _crawledMovieRepository =
            new CrawledMovieRepository();

        private readonly CrawledShowRepository _crawledShowRepository =
            new CrawledShowRepository();

        private readonly GoogleImdbCrawler _googleImdbCrawler = new GoogleImdbCrawler();
        private readonly ImdbCrawler _imdbCrawler = new ImdbCrawler();
        private readonly MovieRepository _movieRepository = new MovieRepository();

        public void CrawlCinema()
        {
            Logger.Info("Crawling shows");
            List<CrawledShow> crawledShows = ExtractShows();
            _crawledShowRepository.Save(crawledShows);
            List<CrawledMovie> crawledMovies = GetDistinctCrawledMovies(crawledShows);
            _crawledMovieRepository.Save(crawledMovies);
            // TODO: Only crawl details for those without imdb ID:
            Logger.Info("Crawling details");
            CrawlDetails(crawledMovies);
            Logger.Info("Crawling imdb IDs");
            CrawlImdbIds(crawledMovies);
            crawledMovies.RemoveAll(x => x.NoImdb());
            _crawledMovieRepository.Save(crawledMovies);
            crawledShows.RemoveAll(
                show =>
                crawledMovies.Any(
                    movie =>
                    show.CrawledMovieId == movie.CrawledMovieId &&
                    movie.NoImdb()));
            _crawledShowRepository.Save(crawledShows);
            // TODO: Only crawl IMDB details for those without IMDB details:
            Logger.Info("Crawling imdb details");
            List<Movie> movies = CrawlImdbDetails(crawledMovies);
            Logger.Info("Saving to movie repo");
            _movieRepository.Save(movies);
            Logger.Info("Saving to read store");
            _cinemaShowReadStore.Save(
                crawledShows.Select(
                    cs => MapToCinemaShowReadModel(cs, crawledMovies, movies))
                            .Where(x => x != null));
            Logger.Info("Crawling done");

            // TODO: Take only the following 3 days.
            // TODO: Run once per day.
        }

        private CinemaShowReadModel MapToCinemaShowReadModel(
            CrawledShow crawledShow,
            IEnumerable<CrawledMovie> crawledMovies,
            IEnumerable<Movie> movies)
        {
            CrawledMovie crawledMovie =
                crawledMovies.SingleOrDefault(
                    x => x.CrawledMovieId == crawledShow.CrawledMovieId);
            if (crawledMovie == null)
                return null;
            Movie movie = movies.Single(x => x.ImdbId == crawledMovie.ImdbId);
            return new CinemaShowReadModel
                {
                    CinemaId = 1,
                    CinemaUrl = "http://www.paradisbio.dk",
                    ImdbRating = movie.ImdbRating,
                    ImdbUrl = "http://www.imdb.com/title/" + movie.ImdbId,
                    ShowTime = crawledShow.ShowTime,
                    ShowUrl = "http://www.paradisbio.dk/" + crawledShow.CrawledShowUrl,
                    Title = crawledShow.ShowTitle,
                    MovieUrl = "http://www.paradisbio.dk/" + crawledShow.CrawledMovieUrl
                };
        }

        private List<CrawledShow> ExtractShows()
        {
            string html =
                CrawlerUtility.Crawl("http://www.paradisbio.dk/program.asp?bio=aarhusc");
            return HtmlParser.ExtractShows(html);
        }

        private void CrawlDetails(CrawledMovie movie)
        {
            string html =
                CrawlerUtility.Crawl("http://www.paradisbio.dk/" + movie.CrawledMovieUrl);

            const string detailsSelector =
                @"table#top_tabel td.forside_bg table table";
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode detailsNode =
                htmlDocument.DocumentNode.QuerySelectorAll(detailsSelector).Last();

            foreach (HtmlNode rowNode in detailsNode.QuerySelectorAll("tr"))
            {
                IEnumerable<HtmlNode> columnNodes = rowNode.QuerySelectorAll("td");
                string key = columnNodes.First().InnerText;
                if (key.StartsWith("Originaltitel"))
                    movie.OriginalTitle = columnNodes.Last().InnerText;
                if (key.StartsWith("Læs mere"))
                {
                    foreach (HtmlNode link in columnNodes.Last().QuerySelectorAll("a"))
                    {
                        if (link.InnerText.Contains("IMDb"))
                        {
                            string imdbUrl = link.Attributes["href"].Value;
                            movie.ImdbId = GoogleImdbCrawler.ExtractImdbId(imdbUrl);
                        }
                    }
                }
            }
        }

        private static List<CrawledMovie> GetDistinctCrawledMovies(
            IEnumerable<CrawledShow> shows)
        {
            return shows.Distinct(new CrawledShowComparer())
                        .Select(MapDistinctShowToCrawledMovie)
                        .OrderBy(x => x.CrawledMovieId)
                        .ToList();
        }

        private static CrawledMovie MapDistinctShowToCrawledMovie(CrawledShow x)
        {
            return new CrawledMovie
                {
                    CrawledMovieId = x.CrawledMovieId,
                    CrawledMovieUrl = x.CrawledMovieUrl,
                    Title = x.ShowTitle
                };
        }

        private void CrawlImdbId(CrawledMovie movie)
        {
            movie.ImdbId =
                _googleImdbCrawler.QueryImdbId(movie.OriginalTitle ?? movie.Title);
        }

        private void CrawlImdbIds(IEnumerable<CrawledMovie> movies)
        {
            List<CrawledMovie> moviesWithoutImdbId =
                movies.Where(x => x.NoImdb()).ToList();
            foreach (CrawledMovie movie in moviesWithoutImdbId)
                CrawlImdbId(movie);
        }

        private void CrawlDetails(IEnumerable<CrawledMovie> movies)
        {
            foreach (CrawledMovie movie in movies)
                CrawlDetails(movie);
        }

        private List<Movie> CrawlImdbDetails(
            IEnumerable<CrawledMovie> crawledMovies)
        {
            List<string> imdbIds =
                crawledMovies.Select(crawledMovie => crawledMovie.ImdbId)
                             .Distinct()
                             .OrderBy(x => x)
                             .ToList();
            Logger.Info("Crawling IMDB ids: " + string.Join(", ", imdbIds));
            var result = new List<Movie>();
            foreach (string imdbId in imdbIds)
            {
                result.Add(_imdbCrawler.CrawlImdb(imdbId));
            }

            return result;
        }
    }
}