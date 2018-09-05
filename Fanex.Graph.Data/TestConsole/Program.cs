using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = ReadFile(@"C:\Users\simon.huynh\Desktop\result500.csv").Distinct().ToArray();

            var data = Post<IEnumerable<int[]>>("http://localhost:51974/api/MiddleService/GroupForCustId", arr);

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

        public static IEnumerable<int> ReadFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var q = new Queue<int>();

                while (!reader.EndOfStream)
                {
                    q.Enqueue(int.Parse(reader.ReadLine()));
                }

                return q;
            }
        }

        public class Group
        {
            public int CustId { get; set; }

            public int GroupId { get; set; }
        }
    }
}