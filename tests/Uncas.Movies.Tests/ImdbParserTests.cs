using System;
using NUnit.Framework;
using Uncas.Movies.Web.Crawling;
using Uncas.Movies.Web.Models;

namespace Uncas.Movies.Tests
{
    [TestFixture]
    public class ImdbParserTests
    {
        [Test]
        public void Parse_Movie()
        {
            Movie movie = ImdbParser.Parse(HtmlParserTests.GetResourceString("imdb.json"));

            Assert.AreEqual("The Hunt", movie.Title);
            Assert.AreEqual("Thomas Vinterberg", movie.Director);
            Assert.AreEqual(TimeSpan.FromMinutes(115d), movie.GetRuntimeAsTimeSpan());
            Assert.AreEqual("Action, Adventure, Fantasy", movie.Genre);
            Assert.AreEqual("R", movie.Rated);
        }
    }
}