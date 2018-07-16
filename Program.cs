using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace searchfight
{
    class Program
    {
        static void Main(string[] queryTerms)
        {
            IResultsFetcher[] fetchers = {
                new GoogleResultsFetcher("Google search"),
                new BingResultsFetcher("Bing search"),
            };
            
            var comparer = new ResultsComparer(fetchers, queryTerms);
            comparer.Compare(queryTerms);
            Console.Write(comparer.GetResultsAsString());
        }
    }
}
