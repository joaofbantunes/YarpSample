using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            var resultTasks = new List<Task<(TimeSpan time, HttpStatusCode status)>>();
            
            for(var i = 0; i < 20; ++i)
            {
                resultTasks.Add(MakeTimedSlowApiRequestAsync(client));
            }

            foreach(var resultTask in resultTasks)
            {
                var result = await resultTask;
                Console.WriteLine($"Got {result.status} in {result.time}");
            }
        }

        private static async Task<(TimeSpan time, HttpStatusCode status)> MakeTimedSlowApiRequestAsync(HttpClient client)
        {
            var stopWatch = Stopwatch.StartNew();
            var result = await client.GetAsync("https://localhost:5001/api/slow");
            stopWatch.Stop();
            return (stopWatch.Elapsed, result.StatusCode);
        }
    }
}
