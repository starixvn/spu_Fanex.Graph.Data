using Fanex.Graph.Data;
using GraphAPI.Criteria;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            DbSessionFactory.Execute(FPS.AddCustomerAndAssociation, criteria);
        }
    }
}