using Fanex.Graph.Data;
using Fanex.Graph.Data.Neo4j;
using System;
using System.Linq;

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

            var data = DbSessionFactory.Query<O>("a").ToArray();

            Console.ReadKey();
        }

        public class O
        {
            public int no { get; set; }

            public string name { get; set; }
        }
    }
}