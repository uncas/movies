using Uncas.Movies.Web;
using Uncas.Movies.Web.Jobs;
using WebActivator;
using WebBackgrounder;

[assembly: PostApplicationStartMethod(typeof (JobSetup), "Start")]
[assembly: ApplicationShutdownMethod(typeof (JobSetup), "Shutdown")]

namespace Uncas.Movies.Web
{
    public static class JobSetup
    {
        private static readonly JobManager JobManager = CreateJobWorkersManager();

        public static void Start()
        {
            JobManager.Start();
        }

        public static void Shutdown()
        {
            JobManager.Dispose();
        }

        private static JobManager CreateJobWorkersManager()
        {
            var jobs = new[] {new MovieJob()};
            var coordinator = new SingleServerJobCoordinator();
            return new JobManager(jobs, coordinator);
        }
    }
}