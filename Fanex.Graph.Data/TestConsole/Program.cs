using Fanex.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            LogService.Info("gi cung duoc");

            Console.WriteLine("done!");

            Console.ReadLine();
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
                    Console.WriteLine($"Error posting {await response.Content.ReadAsStringAsync()}\n\n\n");
                }
            }
        }

        public static IEnumerable<T> ReadFile<T>(string filePath, Func<string, T> mapper)
        {
            using (var reader = new StreamReader(filePath))
            {
                var q = new Queue<T>();

                while (!reader.EndOfStream)
                {
                    q.Enqueue(mapper(reader.ReadLine()));
                }

                return q;
            }
        }

        public static T Pick<T>(this T? a) where T : struct
        {
            if (a == null)
            {
                return default(T);
            }

            return a.Value;
        }

        public class Customer
        {
            public int? Id { get; set; }

            public string Username { get; set; }

            public string Username2 { get; set; }

            public int? RoleId { get; set; }

            public int? Currency { get; set; }

            public string CurrencyName { get; set; }

            public int? SubscriberId { get; set; }

            public int? FpsId { get; set; }

            public bool? Deposit { get; set; }

            public DateTime? CreatedDate { get; set; }

            public static Customer Parse(string row)
            {
                try
                {
                    var arr = row.Split(',');

                    return new Customer
                    {
                        Id = int.Parse(arr[0]),
                        Username = arr[1],
                        Username2 = arr[2],
                        RoleId = int.Parse(arr[3]),
                        Currency = int.Parse(arr[4]),
                        CurrencyName = arr[5],
                        SubscriberId = int.Parse(arr[6]),
                        FpsId = int.Parse(arr[7]),
                        Deposit = bool.Parse(arr[8]),
                        CreatedDate = DateTime.Parse(arr[9])
                    };
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error parsing {row}");
                    return null;
                }
            }
        }

        public class Association
        {
            public int Customer { get; set; }

            public int Device { get; set; }

            public DateTime CreatedDate { get; set; }

            public static Association Parse(string row)
            {
                try
                {
                    var arr = row.Split(',');

                    return new Association
                    {
                        Customer = int.Parse(arr[0]),
                        Device = int.Parse(arr[1]),
                        CreatedDate = DateTime.Parse(arr[2])
                    };
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error parsing {row}");
                    return null;
                }
            }
        }

        public class AddCustomerAndAssociationCriteria
        {
            private string username2;
            private int? roleId;
            private int? currencyId;
            private string currencyName;

            public int DeviceId { get; set; }

            public int CustId { get; set; }

            public string Username { get; set; }

            public string Username2
            {
                get => username2 == string.Empty ? null : username2;
                set { username2 = value; }
            }

            public int? RoleId
            {
                get => roleId == 0 ? null : roleId;

                set { roleId = value; }
            }

            public int? CurrencyId
            {
                get => currencyId == 0 ? null : currencyId;

                set { currencyId = value; }
            }

            public string CurrencyName
            {
                get => currencyName == string.Empty ? null : currencyName;
                set { currencyName = value; }
            }

            public int SubscriberId { get; set; }

            public int FpsAccountId { get; set; }

            public bool IsDeposit { get; set; }

            public string CreatedDate { get; set; }
        }
    }
}