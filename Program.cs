using System;
using System.IO;
//using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace searchfight
{
    interface IResultsFetcher
    {
        Task<string> FetchContents(string query);
        Task<long> GetNumberOfResults(string query);
        long ParseNumberOfResults(string contents);
    }

    class GoogleResultsFetcher : IResultsFetcher
    {
        public async Task<string> FetchContents(string query)
        {
            using(var client = new HttpClient())
            using(var response = await client.GetAsync($"https://www.google.com.pe/search?q={query}"))
            {
                var contents = await response.Content.ReadAsStringAsync();
                return contents;
            }
        }
        
        public async Task<long> GetNumberOfResults(string query)
        {
            var contents = await this.FetchContents(query);
            return this.ParseNumberOfResults(contents);
        }

        public long ParseNumberOfResults(string contents)
        {
            //the correct way would be to use an HTML parser
            //to get the resultStats DOM element, but since we're
            //forbidden to use 3rd party libraries, the simplest way
            //is using a regex to math the results string and extract the number

            //the following regex supports both english and spanish
            //(or any other language that has the same stem)
            var pattern = @"(\d{1,3}(,\d{3})*(\.\d+)?) result\w+";
            var results = Regex.Match(contents,pattern);
            if(!results.Success)
            {
                throw new NotSupportedException("Unable to find a match.");
            }
            
            //results[0] -> the full match
            //results[1] -> the first group (the one we want)
            return Int64.Parse(results.Groups[1].Value,
                                //google uses commas to separate thousands
                                System.Globalization.NumberStyles.AllowThousands);
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            IResultsFetcher resultsFetcher = new GoogleResultsFetcher();
            var query = "send bobs and vegana pls";
            var numberOfResults = await resultsFetcher.GetNumberOfResults(query);
            Console.WriteLine($"search of \"{query}\" -> {numberOfResults} results.");
        }
    }
}
