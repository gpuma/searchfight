using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchfight
{
    class ResultsComparer
    {
        IResultsFetcher[] Fetchers { get; set; }

        //array of tuples (C# 7)
        //name of fetcher, name of winner query term, number of results for winner query
        (string, string, long)[] winners;
        (string, string, long) totalWinner;
        long[,] resultsMatrix;
        string[] Queries { get; set; }

        public ResultsComparer(IResultsFetcher[] fetchers, string[] queries)
        {
            Fetchers = fetchers;
            var foundDuplicates = fetchers.GroupBy(x => x.Name).Any(x => x.Count() > 1);
            if (foundDuplicates)
            {
                throw new InvalidOperationException("Fetchers must have different names.");
            }
            winners = new(string, string, long)[fetchers.Length];
        }

        public void Compare(string[] queries)
        {
            this.Queries = queries;
            resultsMatrix = new long[Fetchers.Length, Queries.Length];
            
            //rows: fetchers
            //columns: queries
            for (int i = 0; i < Fetchers.Length; i++)
            {
                long maxValue = -1;
                var maxQueryIndex = -1;
                for (int j = 0; j < Queries.Length; j++)
                {
                    //blocking call since we need to wait
                    //for all the results in order to determine the winner
                    var task = Fetchers[i].GetNumberOfResults(Queries[j]);
                    task.Wait();
                    resultsMatrix[i, j] = task.Result;
                    
                    if (resultsMatrix[i, j] > maxValue)
                    {
                        maxValue = resultsMatrix[i, j];
                        maxQueryIndex = j;
                    }
                }
                winners[i] = (Fetchers[i].Name, Queries[maxQueryIndex], maxValue);
            }

            //total winner
            totalWinner = (from w in winners
                           orderby w.Item3 descending
                           select w).FirstOrDefault();
        }

        public string GetResultsAsString()
        {
            var strBuilder = new StringBuilder();

            //results by query
            for (int j = 0; j < resultsMatrix.GetLength(1); j++)
            {
                strBuilder.Append($"{Queries[j]}: ");
                for (int i = 0; i < resultsMatrix.GetLength(0); i++)
                {
                    strBuilder.Append($"{Fetchers[i].Name}: {resultsMatrix[i, j]}, ");
                }
                strBuilder.Append(Environment.NewLine);
            }

            //winners
            foreach (var win in winners)
            {
                strBuilder.Append($"{win.Item1} winner: {win.Item2}{Environment.NewLine}");
            }

            //final winner
            strBuilder.Append($"Total winner: {totalWinner.Item2}");

            return strBuilder.ToString();
        }
    }
}