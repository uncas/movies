using System;
using System.Threading.Tasks;
using Uncas.Movies.Web.Crawling;
using WebBackgrounder;

namespace Uncas.Movies.Web.Jobs
{
    public class MovieJob : Job
    {
        private static bool _alreadyRun;

        public MovieJob() : base("MovieJob", TimeSpan.FromSeconds(10d))
        {
        }

        public override Task Execute()
        {
            return new Task(Work);
        }

        private void Work()
        {
            if (_alreadyRun)
                return;
            _alreadyRun = true;
            new CinemaCrawler().CrawlCinema();
        }
    }
}