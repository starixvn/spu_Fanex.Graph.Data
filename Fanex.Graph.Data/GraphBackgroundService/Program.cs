using Fanex.Graph.Data;
using Fanex.Graph.Data.Neo4j;
using System.IO;
using System.ServiceProcess;

namespace GraphBackgroundService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            DbSessionFactory.Init()
                .RegisterConfigs(AppSettings.DB_MiddleService)
                .RegisterXmlFile("~/App_Data")
                .RegisterDriverFactory(new Neo4jDriverFactory())
                .RegisterCommandBuilder(new Neo4jCommandBuilder("~/App_Data/scripts"))
                .RegisterCommandExecutor(new Neo4jExecutor())
                .Run();

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Utilities.ResolvePathForFile("~/GraphBackgroundService.exe.config")));

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new GraphService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}