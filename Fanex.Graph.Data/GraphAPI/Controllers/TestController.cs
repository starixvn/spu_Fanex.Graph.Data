using Fanex.Graph.Data;
using GraphAPI.Factory;
using System.Web;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public object Hello()
        {
            try
            {
                return DbSessionFactory.ExecuteSingle<string>(Test.HelloApi);
            }
            catch (System.Exception ex)
            {
                return ex.Message + "\n" + ex.StackTrace;
            }
        }
    }
}