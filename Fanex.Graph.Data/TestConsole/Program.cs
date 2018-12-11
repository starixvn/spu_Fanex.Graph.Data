using Fanex.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FillingGapDataTool
{
    internal static class Program
    {
        private static HttpClient web = new HttpClient();

        private static void Main(string[] args)
        {
            var customerCsv = "";
            var associationCsv = "";
            var url = "";
            var skippedRow = 0;

            Console.WriteLine("Reading configuration...");
            customerCsv = ConfigurationManager.AppSettings[nameof(customerCsv)];
            associationCsv = ConfigurationManager.AppSettings[nameof(associationCsv)];
            url = ConfigurationManager.AppSettings[nameof(url)];
            skippedRow = Convert.ToInt32(ConfigurationManager.AppSettings[nameof(skippedRow)]);

            Console.WriteLine("Reading files...");
            var customers = ReadFile(customerCsv, Customer.Parse);
            var associations = ReadFile(associationCsv, Association.Parse);         
            if (customers.Any(c => c == null) || associations.Any(a => a == null))
            {
                Console.ReadLine();
                return;
            }

            var totalAssociation = associations.Count();

            Console.WriteLine("Total association: " + totalAssociation + " rows");
            int count = 0;
            var custDict = customers.ToDictionary(c => c.Id);

            foreach (var item in associations.Skip(skippedRow)) 
            {
                custDict.TryGetValue(item.Customer, out var customer);

                var criteria = new AddCustomerAndAssociationCriteria
                {
                    CustId = item.Customer,
                    DeviceId = item.Device,
                    RoleId = Pick(customer?.RoleId),
                    Username = customer?.Username,
                    Username2 = customer?.Username2
                };

                var tsk = Post(url, criteria);
                Task.WaitAll(tsk);
                count += 1;
                Console.WriteLine(count + " Requests were sent successfully!");
            }

            Console.WriteLine("Process done!");

            Console.ReadLine();
        }

        public static async Task<TResult> Post<TResult>(string url, object param)
        {
            var response = await web.PostAsJsonAsync(url, param).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return default(TResult);
            }

            return await response.Content.ReadAsAsync<TResult>().ConfigureAwait(false);
        }

        public static async Task Post(string url, object param)
        {
            var response = await web.PostAsJsonAsync(url, param).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error posting {await response.Content.ReadAsStringAsync()}\n\n\n");
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
                        CreatedDate = DateTime.Parse(arr[4])
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

            public string CreatedDate { get; set; }
        }
    }
}