using Fanex.Graph.Data;
using Fanex.Logging;
using GraphAPI.Criteria;
using System;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        [HttpPost]
        public object AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            criteria.CreatedDate = DateTime.Now.ToString();
            return DbSessionFactory.Execute(FPS.AddCustomerAndAssociation, criteria);
        }

        [HttpPost]
        public object AddEvidence(AddEvidenceCriteria criteria)
        {
            criteria.CreatedDate = DateTime.Now.ToString();
            return DbSessionFactory.Execute(FPS.AddCustomerAndEvidence, criteria);
        }

        [HttpPost]
        public object RemoveEvidenceByCustomerList(RemoveEvidenceByCustomerListCriteria criteria)
        {
            return DbSessionFactory.Execute(FPS.RemoveEvidenceByCustomerList, criteria);
        }
    }
}