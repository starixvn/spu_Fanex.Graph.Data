using GraphAPI.Criteria;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                var criteria = new AddCustomerAndAssociationCriteria
                {
                    CustId = -i,
                    Username = $"test {i}",
                    RoleId = 1,
                    CurrencyId = 16,
                    CurrencyName = "Yen",
                    SubscriberId = 169,
                    DeviceId = -i,
                };

                var tsk = Post("http://localhost:51974/api/FPS/AddAssociation", criteria);

                Task.WaitAll(tsk);

                criteria.DeviceId = -2;

                tsk = Post("http://localhost:51974/api/FPS/AddAssociation", criteria);

                Task.WaitAll(tsk);
            }
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

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidDataException();
                }
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