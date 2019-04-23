namespace GraphAPI.Controllers
{
    using Fanex.Graph.Data;
    using Fanex.Graph.Data.Models;
    using GraphAPI.Factory;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    public class MiddleServiceController : ApiController
    {
        [HttpPost]
        public IEnumerable<Group> GroupForCustId(IEnumerable<int> custIds)
        {
            IEnumerable<Group> result = null;
            try
            {
                result = DbSessionFactory.Execute<Group>(MiddleSerivce.GroupCustomer, new { CustIdList = custIds }).Data;
            }
            catch (Exception ex)
            {
                Logger.Error($"GroupForCustId - {ex.Message}", ex);
            }

            return result;
        }
    }
}