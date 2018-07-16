using System;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResultsFetcher
    {
        string Name { get; set; }
        Task<long> GetNumberOfResults(string query);
    }
}