using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace searchfight
{
    class HttpRequestFetcher : IResourceFetcher
    {
        public async Task<string> FetchContentsAsString(string uri)
        {
            using(var client = new HttpClient())
            using(var response = await client.GetAsync(uri))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}