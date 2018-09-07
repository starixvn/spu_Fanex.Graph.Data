using GraphAPI.Criteria;
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
            var arr = new TEst
            {
               CurrencyId = 2,
               CurrencyName = "RM",
               CustId = 329981,
               DeviceId = 128,
               FpsAccountId = 6313,
               IsDeposit = true,
               RoleId = 5,
               SubscriberId = 0,
               Username = "fpsw_vvsony",
               Username2 = ""
            };

            var data = Post("http://localhost:51974/api/FPS/AddAssociation", arr);

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

        public class TEst
        {
            public int DeviceId { get; set; }

            public int CustId { get; set; }

            public string Username { get; set; }

            public string Username2 { get; set; }

            public int RoleId { get; set; }

            public int CurrencyId { get; set; }

            public string CurrencyName { get; set; }

            public int SubscriberId { get; set; }

            public int FpsAccountId { get; set; }

            public bool IsDeposit { get; set; }
        }
    }
}