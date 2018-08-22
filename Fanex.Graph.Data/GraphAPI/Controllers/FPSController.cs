using Fanex.Graph.Data;
using GraphAPI.Models;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public void AddAssociation(Association parameters)
        {
            DbSessionFactory.Execute("AddAssociation", parameters);
        }
    }
}