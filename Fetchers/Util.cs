using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace searchfight
{
    class Util
    {
        public static string preprocessQuery(string queryTerm)
        {
            //nothing to do
            if(!queryTerm.Any(Char.IsWhiteSpace))
            {
                return queryTerm;
            }
            //'+' is the standard character to separate whitespace in search queries 
            var newQueryTerm = Regex.Replace(queryTerm,@"\s+","+");
            //since it has spaces it means the user wants an exact search, so we re-add quotes
            return $"\"{newQueryTerm}\"";
        }
    }
}