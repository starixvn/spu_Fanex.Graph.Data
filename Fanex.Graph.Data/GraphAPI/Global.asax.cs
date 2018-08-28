using Fanex.Graph.Data;
using GraphAPI.Neo4j;
using System.Web.Http;

namespace GraphAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DbSessionFactory.Init()
                .RegisterConfigs("DB.MiddleService")
                .RegisterXmlFile("~/App_Data")
                .RegisterDriverFactory(new Neo4jDriverFactory())
                .RegisterCommandBuilder(new Neo4jCommandBuilder("~/App_Data/scripts"))
                .RegisterCommandExecutor(new Neo4jExecutor())
                .Run();
        }
    }
}