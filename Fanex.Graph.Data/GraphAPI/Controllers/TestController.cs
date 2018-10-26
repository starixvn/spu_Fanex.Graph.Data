using Fanex.Graph.Data;
using Fanex.Logging;
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
                LogService.Info("Tao day ne!!!!!");
                LogService.Error("Tao day ne - nhung ma bi loi~ roi !!!!!");
                return DbSessionFactory.ExecuteSingle<string>(Test.HelloApi);
            }
            catch (System.Exception ex)
            {
                return ex.Message + "\n" + ex.StackTrace;
            }
        }
    }
}