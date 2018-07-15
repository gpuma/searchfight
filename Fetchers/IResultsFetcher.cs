using System;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResultsFetcher
    {
        Task<long> GetNumberOfResults(string query);
        long ParseNumberOfResults(string contents);
    }
}