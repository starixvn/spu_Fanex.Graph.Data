using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = Post<IEnumerable<int[]>>("http://localhost:51974/api/MiddleService/GroupForCustId", new HashSet<int> { 31675706, 31939885, 31936354, 31626123 }).Result.ToArray();

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
    }
}