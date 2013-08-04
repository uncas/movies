using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Uncas.Movies.Web.Crawling;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class HtmlParserTests
    {
        [Test]
        public void ExtractShows_ParadisBio_309()
        {
            Assembly assembly = GetType().Assembly;
            Stream manifestResourceStream =
                assembly.GetManifestResourceStream(
                    "Uncas.Movies.Tests.Html.ParadisbioProgram.html");
            if (manifestResourceStream == null)
                Assert.Fail("Resource not found.");
            var htmlReader = new StreamReader(manifestResourceStream);
            string html = htmlReader.ReadToEnd();

            IEnumerable<CrawledShow> shows = HtmlParser.ExtractShows(html);

            Assert.AreEqual(309, shows.Count());
            CrawledShow firstShow = shows.First();
            Assert.AreEqual("Søndag i Paradis - Drengen med Cyklen", firstShow.ShowTitle);
            Assert.AreEqual(
                "bestil_1.asp?forestillingid=40965&bio=aarhusc",
                firstShow.CrawledShowUrl);
            Assert.AreEqual(
                "film_omtale.asp?filmid=5844&bio=aarhusc",
                firstShow.CrawledMovieUrl);
        }
    }
}