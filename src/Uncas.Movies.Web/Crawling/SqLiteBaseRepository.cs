using System.Data.SQLite;

namespace Uncas.Movies.Web.Crawling
{
    public abstract class SqLiteBaseRepository
    {
        protected readonly string DbFile;

        protected SqLiteBaseRepository(string dbFile)
        {
            DbFile = dbFile;
        }

        protected SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}