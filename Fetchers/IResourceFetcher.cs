using System;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResourceFetcher
    {
        Task<string> FetchContentsAsString(string uri);
    }
}