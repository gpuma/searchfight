using System;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResultsFetcher
    {
        string Name { get; }
        Task<long> GetNumberOfResults(string query);
    }
}