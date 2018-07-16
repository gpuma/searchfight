using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace searchfight
{
    //another alternative would be to use the Bing API
    class BingResultsFetcher : IResultsFetcher
    {
        public string Name { get; set; }

        public BingResultsFetcher(string name)
        {
            Name = name;
        }

        public async Task<long> GetNumberOfResults(string query)
        {
            var contents = await new HttpRequestFetcher()
                .FetchContentsAsString($"https://www.bing.com/search?q={Util.preprocessQuery(query)}");
            return this.ParseNumberOfResults(contents);
        }

        public long ParseNumberOfResults(string contents)
        {
            var pattern = @"(\d{1,3}(.\d{3})*(\.\d+)?) result\w+";
            var results = Regex.Match(contents,pattern);
            if(!results.Success)
            {
                throw new NotSupportedException("Unable to find a match.");
            }
            
            //results[0] -> the full match
            //results[1] -> the first group (the one we want)
            var dotSeparatedResult = results.Groups[1].Value.ToString();
            //bing uses dots as thousands separators, so we remove them
            var rawResult = dotSeparatedResult.Replace(".", string.Empty);
            return Int64.Parse(rawResult);
        }
    }
}
