using System;
using System.Threading.Tasks;

namespace searchfight
{
    interface IResultsComparer
    {
        void Compare(string[] queries);
    }
}