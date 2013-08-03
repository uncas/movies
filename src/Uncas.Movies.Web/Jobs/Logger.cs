using System;
using System.Web;
using Elmah;

namespace Uncas.Movies.Web.Jobs
{
    public class Logger
    {
        public static void Info(string message)
        {
            Log(new Exception("INFO: " + message));
        }

        public static void Log(Exception exception)
        {
            ErrorLog.GetDefault(HttpContext.Current).Log(new Error(exception));
        }
    }
}