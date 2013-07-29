using System;
using System.Threading;
using System.Threading.Tasks;
using WebBackgrounder;

namespace Uncas.Movies.Web.Jobs
{
    public class MovieJob : Job
    {
        public MovieJob() : base("MovieJob", TimeSpan.FromMinutes(60d))
        {
        }

        public override Task Execute()
        {
            return new Task(Work);
        }

        private static void Work()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1d));
        }
    }
}