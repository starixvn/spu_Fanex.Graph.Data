namespace GraphAPI.Controllers
{
    using Fanex.Graph.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    public class MiddleServiceController : ApiController
    {
        [HttpPost]
        public IEnumerable<object> GroupForCustId(IEnumerable<int> custIds)
        {
            var ids = new HashSet<int>(custIds);

            var q = new Queue<object>();

            while (ids.Count != 0)
            {
                var key = ids.First();

                ids.Remove(key);

                var grouped = DbSessionFactory.Execute<int>(MiddleSerivce.GroupCustomer, new { CustId = key.ToString(), CustIdList = ids.Select(a => a.ToString()) });

                var set = new HashSet<int>(grouped)
                {
                    key
                };

                q.Enqueue(set);

                ids.ExceptWith(set);
            }

            return q;
        }
    }
}