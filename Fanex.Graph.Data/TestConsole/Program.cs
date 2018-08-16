using Fanex.Graph.Data;
using Fanex.Graph.Data.Neo4j;
using System;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DbSessionFactory.Init()
                .RegisterConfigs("db1")
                .RegisterXmlFile("~/data")
                .RegisterDriverFactory(new Neo4jDriverFactory())
                .RegisterCommandBuilder(new Neo4jCommandBuilder("~/script"))
                .RegisterCommandExecutor(new Neo4jExecutor())
                .Run();

            var data = DbSessionFactory.Execute<O>("b");

            foreach (var item in data)
            {
                Console.WriteLine($"no: {item.No}, name: {item.Name}");
            }

            Console.ReadKey();
        }

        public class O
        {
            public int No { get; set; }

            public string Name { get; set; }
        }
    }
}