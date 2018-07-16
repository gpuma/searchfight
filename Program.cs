using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace searchfight
{
    class Program
    {
        //async Main only supported in C# 7
        static void Main(string[] args)
        {
            // IResultsFetcher googleFetcher = new GoogleResultsFetcher();
            // IResultsFetcher bingFetcher = new BingResultsFetcher();
            // var query = "we live in a society meme";
            // var googleResults = await googleFetcher.GetNumberOfResults(query);
            // var bingResults = await bingFetcher.GetNumberOfResults(query);
            // Console.WriteLine($"google \"{query}\" -> {googleResults} results.");
            // Console.WriteLine($"bing \"{query}\" -> {bingResults} results.");

            // // foreach(string a in args)
            // // {
            // //     Console.WriteLine(a);
            // // }

            IResultsFetcher[] fetchers = {new GoogleResultsFetcher("Google search"), new BingResultsFetcher("Bing search")};
            var comparer = new ResultsComparer(fetchers, args);
            comparer.Compare();
            comparer.DisplayResults();
        }
    }
}
