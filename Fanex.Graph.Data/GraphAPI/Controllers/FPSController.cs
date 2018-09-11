using Fanex.Graph.Data;
using GraphAPI.Criteria;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public object AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            return DbSessionFactory.Execute(FPS.AddCustomerAndAssociation, criteria);
        }
    }
}