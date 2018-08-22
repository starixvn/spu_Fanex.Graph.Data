using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = Post<IEnumerable<int[]>>("http://localhost:51974/api/MiddleService/GroupForCustId", new[] { 31896118,
                    32050611,
                    32050228,
                    32050100,
                    32049784,
                    32042604,
                    31643703,
                    32042302,
                    32042074,
                    32041879,
                    32041853});

            Task.WaitAll(data);

            Console.ReadKey();
        }

        public static async Task<TResult> Post<TResult>(string url, object param)
        {
            using (var web = new HttpClient())
            {
                var response = await web.PostAsJsonAsync(url, param).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return default(TResult);
                }

                return await response.Content.ReadAsAsync<TResult>().ConfigureAwait(false);
            }
        }

        public static async Task Post(string url, object param)
        {
            using (var web = new HttpClient())
            {
                var response = await web.PostAsJsonAsync(url, param).ConfigureAwait(false);
            }
        }
    }
}