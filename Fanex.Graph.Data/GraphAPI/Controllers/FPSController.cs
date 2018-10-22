using EasyNetQ;
using Fanex.Graph.Data;
using GraphAPI.Criteria;
using GraphAPI.Service.Implement.FPS;
using GraphAPI.Service.Interface;
using System;
using System.Linq;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static IAdvancedBus _advancedBus;
        private static IAsyncQueue<AddCustomerAndAssociationCriteria> queue;

        static FPSController()
        {
            queue = new EasyNetQService();

            queue.AddHandler(InsertCustomer);
        }

        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
#if DEBUG
            criteria.CustId = 32297449;
            criteria.Username = "BV890988888";
            criteria.Username2 = null;
            criteria.RoleId = 1;
            criteria.CurrencyId = 16;
            criteria.CurrencyName = "Yen";
            criteria.SubscriberId = 169;
            criteria.DeviceId = 59671118;
#endif
            criteria.CreatedDate = DateTime.Now.ToString();

            queue.Enqueue(criteria);
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

        [HttpPost]
        public object UpdateUsername2(UpdateUsername2Criteria criteria)
        {
            return DbSessionFactory.Execute(FPS.UpdateCustomerUsername2, criteria);
        }

        private static void InsertCustomer(AddCustomerAndAssociationCriteria criteria)
        {
            var custId = DbSessionFactory.Query<int>(FPS.GetCustomerById, new { criteria.CustId });
            var deviceId = DbSessionFactory.Query<int>(FPS.GetDeviceById, new { criteria.DeviceId });
            var association = DbSessionFactory.Query<int>(FPS.CheckExistedAssociation, new { criteria.CustId, criteria.DeviceId });

            DbSessionFactory.Execute(FPS.AddAssociation, criteria);

            if (custId.Any())
            {
                if (deviceId.Any())
                {
                    // case acc cu, device cu
                    if (association.Any())
                    {
                        DbSessionFactory.Execute(FPS.UpdateCustomerUsername2, new { criteria.CustId, criteria.Username2 });
                    }
                    else
                    {
                        DbSessionFactory.Execute(Jobs.MergeGroup, new { criteria.CustId, criteria.DeviceId });
                    }
                }
            }
            else
            {
                if (deviceId.Any())
                {
                    // case acc moi, device cu
                    DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, criteria.DeviceId });
                }
                else
                {
                    // case acc moi, device moi
                    DbSessionFactory.Execute(FPS.CreateNewGroup, new { criteria.CustId });
                }
            }
        }
    }
}