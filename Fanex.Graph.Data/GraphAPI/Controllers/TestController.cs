using Fanex.Graph.Data;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public object Hello()
        {
            return DbSessionFactory.ExecuteSingle<string>(Test.HelloApi);
        }
    }
}