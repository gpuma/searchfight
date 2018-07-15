using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace searchfight
{
    class Program
    {      
        static async Task Main(string[] args)
        {
            IResultsFetcher googleFetcher = new GoogleResultsFetcher();
            IResultsFetcher bingFetcher = new BingResultsFetcher();
            var query = "we live in a society meme";
            var googleResults = await googleFetcher.GetNumberOfResults(query);
            var bingResults = await bingFetcher.GetNumberOfResults(query);
            Console.WriteLine($"google \"{query}\" -> {googleResults} results.");
            Console.WriteLine($"bing \"{query}\" -> {bingResults} results.");

            // foreach(string a in args)
            // {
            //     Console.WriteLine(a);
            // }
        }
    }
}
