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
        string[] queries;

        public ResultsComparer(IResultsFetcher[] fetchers, string[] queries)
        {
            Fetchers = fetchers;
            var foundDuplicates = fetchers.GroupBy(x => x.Name).Any(x => x.Count() > 1);
            if(foundDuplicates)
            {
                throw new InvalidOperationException("Fetchers must have different names.");
            }
            winners = new (string, string, long)[fetchers.Length];
            this.queries = queries;
        }
        
        public async void Compare()
        {
            resultsMatrix = new long[Fetchers.Length, queries.Length];
            for(int i=0;i<Fetchers.Length;i++)
            {
                long maxValue = -1;
                var maxQueryIndex = -1;
                for(int j=0;j<queries.Length;j++)
                {
                    //blocking call since we need for the program
                    //to wait on this process
                    var task = Fetchers[i].GetNumberOfResults(queries[j]);
                    task.Wait();
                    
                    resultsMatrix[i,j] = task.Result;
                    if(resultsMatrix[i,j]>maxValue)
                    {
                        maxValue = resultsMatrix[i,j];
                        maxQueryIndex = j;
                    }
                }
                winners[i]=(Fetchers[i].Name, queries[maxQueryIndex], maxValue);
            }

            //total winner
            totalWinner = (from w in winners
                            orderby w.Item3 descending
                            select w).FirstOrDefault();
        }

        public void DisplayResults()
        {
            var strBuilder = new StringBuilder();
            
            //results by query
            for(int j=0;j<resultsMatrix.GetLength(1);j++)
            {
                strBuilder.Append($"{queries[j]}: ");
                for(int i=0;i<resultsMatrix.GetLength(0);i++)
                {
                    strBuilder.Append($"{Fetchers[i].Name}: {resultsMatrix[i,j]} ");
                }
                strBuilder.Append(Environment.NewLine);
            }

            //winners
            foreach(var win in winners)
            {
                strBuilder.Append($"{win.Item1} winner: {win.Item2}{Environment.NewLine}");
            }

            //final winner
            strBuilder.Append($"Total winner: {totalWinner.Item2}");

            //TODOV: return instead of writing here
            Console.Write(strBuilder.ToString());
        }
    }
}