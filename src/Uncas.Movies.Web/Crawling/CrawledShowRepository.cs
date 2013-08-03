using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;

namespace Uncas.Movies.Web.Crawling
{
    public class CrawledShowRepository : SqLiteBaseRepository
    {
        private static List<CrawledShow> _crawledShows;

        public CrawledShowRepository()
            : base(Environment.CurrentDirectory + "\\SimpleDb.sqlite")
        {
        }

        public void Save(IEnumerable<CrawledShow> crawledShows)
        {
            _crawledShows = crawledShows.ToList();
            return;
            if (!File.Exists(DbFile))
                CreateDatabase();

            foreach (CrawledShow show in crawledShows)
                using (SQLiteConnection cnn = GetConnection())
                {
                    cnn.Open();
                    cnn.Execute(
                        @"
INSERT INTO CrawledShow
(CinemaId, CrawledMovieId, ShowTime, CrawledMovieUrl, ShowTitle)
VALUES 
(@CinemaId, @CrawledMovieId, @ShowTime, @CrawledMovieUrl, @ShowTitle);",
                        show);
                }
        }

        private void CreateDatabase()
        {
            using (SQLiteConnection cnn = GetConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"
CREATE TABLE CrawledShow
(
    Id  integer primary key AUTOINCREMENT
    , CinemaId  integer  NOT NULL
    , CrawledMovieId  nvarchar(100)  NOT NULL
    , ShowTime  datetime  NOT NULL
    , CrawledMovieUrl  nvarchar(100)  NOT NULL
    , ShowTitle  nvarchar(100)  NOT NULL
)");
            }
        }
    }
}