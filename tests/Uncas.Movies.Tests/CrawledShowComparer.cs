using System.Collections.Generic;

namespace Uncas.Movies.Tests
{
    public class CrawledShowComparer : IEqualityComparer<CrawledShow>
    {
        public bool Equals(CrawledShow x, CrawledShow y)
        {
            return x.CrawledMovieId.Equals(y.CrawledMovieId);
        }

        public int GetHashCode(CrawledShow obj)
        {
            return obj.CrawledMovieId.GetHashCode();
        }
    }
}