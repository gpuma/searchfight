using System;
using System.IO;
//using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResultsFetcher
    {
        Task<string> GetContents(string query);
        Task<long> GetNumberOfResults(string query);
        Task<long> ParseNumberOfResults(string query);
    }

    class GoogleResultsFetcher : IResultsFetcher
    {
        public async Task<string> GetContents(string query)
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
            throw new NotImplementedException();
            // var request = WebRequest.Create($"https://www.google.com.pe/search?q={query}");
            // byte[] content = null;
            // using(var response = (HttpWebResponse)request.GetResponse())
            // using(var stream = response.GetResponseStream())
            // {
            //     content = stream.reADAS
            // }
            // Console.WriteLine(content);
        }
        public async Task<long> ParseNumberOfResults(string query)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            IResultsFetcher resultsFetcher = new GoogleResultsFetcher();
            using(var writer = new StreamWriter(@"D:\prueba.html"))
            {
                writer.Write(await resultsFetcher.GetContents("bobs and vegana"));
            }            
        }
    }
}
