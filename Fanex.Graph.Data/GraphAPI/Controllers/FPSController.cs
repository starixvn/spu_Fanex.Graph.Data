using Fanex.Graph.Data;
using GraphAPI.Criteria;
using System.Linq;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            var groupIds =  DbSessionFactory.Execute<int>(FPS.AddCustomerAndAssociation, criteria).ToArray();
            if (groupIds.Any())
            {
                var mergeExistingGroupCriteria = new MergeExistingGroupCriteria()
                {
                    GroupIdList = groupIds,
                    CustId = criteria.CustId
                };

                DbSessionFactory.Execute<int>(FPS.MergeExistingGroup, mergeExistingGroupCriteria);
            }
            else
            {
                var addGroupCriteria = new AddNewGroupCriteria() { CustId = criteria.CustId };
                DbSessionFactory.Execute<int>(FPS.AddNewGroup, addGroupCriteria);
            }
        }
    }
}