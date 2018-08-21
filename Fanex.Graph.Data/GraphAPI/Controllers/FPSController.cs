using Fanex.Graph.Data;
using GraphAPI.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public void AddAssociation(object parameters)
        {
            DbSessionFactory.Execute("AddAssociation", parameters);
        }


        [HttpGet]
        public IEnumerable<A> Test()
        {
            return DbSessionFactory.Execute<A>("a");
        }
    }
}