using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Uncas.Movies.Web.Crawling
{
    public class CinemaCrawler
    {
        private readonly CrawledMovieRepository _crawledMovieRepository =
            new CrawledMovieRepository();

        private readonly CrawledShowRepository _crawledShowRepository =
            new CrawledShowRepository();

        private readonly GoogleImdbCrawler _googleImdbCrawler = new GoogleImdbCrawler();
        private readonly ImdbCrawler _imdbCrawler = new ImdbCrawler();

        public void CrawlCinema()
        {
            List<CrawledShow> crawledShows = ExtractShows().ToList();
            _crawledShowRepository.Save(crawledShows);
            List<CrawledMovie> crawledMovies = GetDistinctCrawledMovies(crawledShows);
            _crawledMovieRepository.Save(crawledMovies);
            // TODO: Only crawl details for those without imdb ID:
            CrawlDetails(crawledMovies);
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
            IEnumerable<Movie> movies = CrawlImdbDetails(crawledMovies);
            // TODO: Save to Movie repository
            // TODO: Create and save to read store

            // TODO: Take only the following 3 days.
            // TODO: Run once per day.
        }

        private static IEnumerable<CrawledShow> ExtractShows(string html)
        {
            var crawledShows = new List<CrawledShow>();
            const string dayScheduleSelector =
                "table#top_tabel td.forside_bg table";
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            List<HtmlNode> dayScheduleNodes =
                htmlDocument.DocumentNode.QuerySelectorAll(dayScheduleSelector).ToList();
            foreach (HtmlNode dayScheduleNode in dayScheduleNodes)
            {
                string dayDescription =
                    dayScheduleNode.QuerySelector("tr:nth-child(1) td:nth-child(1)")
                                   .InnerText;
                var dateExpression = new Regex(@"(\d)+[/](\d)+[-](\d)+");
                string dateText = dateExpression.Match(dayDescription).Value;
                DateTime date = DateTime.Parse(dateText, new CultureInfo("da-DK"));
                IEnumerable<HtmlNode> movieNodes =
                    dayScheduleNode.QuerySelectorAll("tr")
                                   .Where((item, index) => index > 0 && index%2 == 0);
                foreach (HtmlNode movieNode in movieNodes)
                {
                    HtmlNode titleNode = movieNode.QuerySelector("td:nth-child(1)");
                    HtmlNode linkNode = titleNode.QuerySelector("a");
                    string title = linkNode.InnerText;
                    string url = linkNode.Attributes["href"].Value;

                    var idExpression = new Regex(@"(\d)+");
                    string id = idExpression.Match(url).Value;

                    string timeText = movieNode.QuerySelector("td:nth-child(3)").InnerText;
                    var timeExpression = new Regex(@"(\d)+[:](\d)+");
                    string time = timeExpression.Match(timeText).Value;

                    string[] timeParts = time.Split(':');
                    int hours = int.Parse(timeParts[0]);
                    int minutes = int.Parse(timeParts[1]);
                    crawledShows.Add(new CrawledShow
                        {
                            ShowTime = date.AddHours(hours).AddMinutes(minutes),
                            CrawledMovieId = id,
                            CrawledMovieUrl = url,
                            ShowTitle = title
                        });
                }
            }

            return crawledShows;
        }

        private IEnumerable<CrawledShow> ExtractShows()
        {
            string html =
                CrawlerUtility.Crawl("http://www.paradisbio.dk/program.asp?bio=aarhusc");
            return ExtractShows(html);
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
            List<CrawledMovie> movies =
                shows.Distinct(new CrawledShowComparer())
                     .Select(
                         x =>
                         new CrawledMovie
                             {
                                 CrawledMovieId = x.CrawledMovieId,
                                 CrawledMovieUrl = x.CrawledMovieUrl,
                                 Title = x.ShowTitle
                             }).OrderBy(x => x.CrawledMovieId).ToList();
            return movies;
        }

        private void CrawlImdbId(CrawledMovie movie)
        {
            movie.ImdbId =
                _googleImdbCrawler.QueryImdbId(movie.OriginalTitle ?? movie.Title);
        }

        private void CrawlImdbIds(IEnumerable<CrawledMovie> movies)
        {
            IEnumerable<CrawledMovie> moviesWithoutImdbId = movies.Where(x => x.NoImdb());
            foreach (CrawledMovie movie in moviesWithoutImdbId)
                CrawlImdbId(movie);
        }

        private void CrawlDetails(IEnumerable<CrawledMovie> movies)
        {
            foreach (CrawledMovie movie in movies)
                CrawlDetails(movie);
        }

        private IEnumerable<Movie> CrawlImdbDetails(
            IEnumerable<CrawledMovie> crawledMovies)
        {
            return crawledMovies.Select(x => _imdbCrawler.CrawlImdb(x.ImdbId));
        }
    }
}