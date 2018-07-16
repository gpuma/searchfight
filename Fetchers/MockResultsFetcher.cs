using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace searchfight
{
    class MockResultsFetcher : IResultsFetcher
    {
        public string Name { get; set; }
        int count = 0;

        public MockResultsFetcher(string name)
        {
            Name = name;
        }

        public Task<long> GetNumberOfResults(string query)
        {
            long result;
            if(count == 0)
                result = 10000;
            else if (count == 1)
                result = 500;
            else
                result = 26;
            count++;
            return Task.FromResult(result);
        }
    }
}