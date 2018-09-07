namespace GraphAPI.Controllers
{
    using Fanex.Graph.Data;
    using GraphAPI.Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    public class MiddleServiceController : ApiController
    {
        [HttpPost]
        public IEnumerable<object> GroupForCustId(IEnumerable<int> custIds)
        {
            return DbSessionFactory
                .Execute<Group>(MiddleSerivce.GroupCustomer, new { CustIdList = custIds });
        }
    }
}