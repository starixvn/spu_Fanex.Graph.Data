using Fanex.Graph.Data;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public void AddAssociation(object parameters)
        {
            DbSessionFactory.Execute("", parameters);
        }
    }
}