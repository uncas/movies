using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class CinemaCrawlerTests
    {
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

        private static List<CrawledMovie> GetCrawledMovies(IEnumerable<CrawledShow> shows)
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

        private readonly GoogleImdbCrawler _googleImdbCrawler = new GoogleImdbCrawler();

        private void CrawlImdbId(CrawledMovie movie)
        {
            movie.ImdbId =
                _googleImdbCrawler.QueryImdbId(movie.OriginalTitle ?? movie.Title);
        }

        private void CrawlImdbIds(IEnumerable<CrawledMovie> movies)
        {
            IEnumerable<CrawledMovie> moviesWithoutImdbId =
                movies.Where(x => string.IsNullOrWhiteSpace(x.ImdbId));
            foreach (CrawledMovie movie in moviesWithoutImdbId)
                CrawlImdbId(movie);
        }

        private void CrawlDetails(IEnumerable<CrawledMovie> movies)
        {
            foreach (CrawledMovie movie in movies)
                CrawlDetails(movie);
        }

        private void CrawlImdbDetails(List<CrawledMovie> crawledMovies)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CrawlEastOfEden()
        {
            // TODO: Consider when to save which data:
            IEnumerable<CrawledShow> crawledShows = ExtractShows();
            List<CrawledMovie> crawledMovies = GetCrawledMovies(crawledShows);
            CrawlDetails(crawledMovies);
            CrawlImdbIds(crawledMovies);
            CrawlImdbDetails(crawledMovies);

            // TODO: Take only the following 3 days.
            // TODO: Run once per day.

            // Algorithm for 'Øst for Paradis':
            // Go through shows and record CinemaShow (date, time, movie id, URL to page, display title)
            // For each distinct CinemaShow where we don't already have the IMDB id
            //   Follow URL to details page
            //   Determine the IMDB id:
            //     If directly on page, then take that!
            //     Else search on google: <originalTitle> site:www.imdb.com
            // For each distinct movie where we do not already have details from IMDB:
            //   http://www.omdbapi.com/?i=tt2106476
            //   Get details from IMDB: imdbRating, official title, director(s), year, genre, rating, imdbID, pictureUrl, plotLine, Actors, Runtime
        }
    }
}