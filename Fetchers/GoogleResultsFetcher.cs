using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace searchfight
{
    class GoogleResultsFetcher : IResultsFetcher
    {
        public string Name { get; set; }

        public GoogleResultsFetcher(string name)
        {
            Name = name;
        }

        public async Task<long> GetNumberOfResults(string query)
        {
            var contents = await new HttpRequestFetcher()
                .FetchContentsAsString($"https://www.google.com.pe/search?q={Util.preprocessQuery(query)}");
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
            var results = Regex.Match(contents, pattern);
            if (!results.Success)
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
}