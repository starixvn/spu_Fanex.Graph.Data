using Fanex.Graph.Data;
using GraphAPI.Criteria;
using System.Linq;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public object AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            var result = DbSessionFactory.Execute<int>(FPS.AddCustomerAndAssociation, criteria);

            var groupIds = result.Data;

            if (groupIds.Any())
            {
                var mergeExistingGroupCriteria = new MergeExistingGroupCriteria
                {
                    GroupIdList = groupIds,
                    CustId = criteria.CustId
                };

                return result + DbSessionFactory.Execute(FPS.MergeExistingGroup, mergeExistingGroupCriteria);
            }
            else
            {
                var addGroupCriteria = new AddNewGroupCriteria() { CustId = criteria.CustId };
                return result + DbSessionFactory.Execute(FPS.AddNewGroup, addGroupCriteria);
            }
        }
    }
}