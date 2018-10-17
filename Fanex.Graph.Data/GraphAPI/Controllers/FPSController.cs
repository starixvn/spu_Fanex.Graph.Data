using Fanex.Graph.Data;
using Fanex.Logging;
using GraphAPI.Criteria;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static ConcurrentQueue<AddCustomerAndAssociationCriteria> queue = new ConcurrentQueue<AddCustomerAndAssociationCriteria>();

        private static Thread thread;

        [HttpPost]
        public object AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            criteria.CustId = 32297449;
            criteria.Username = "BV890988888";
            criteria.Username2 = null;
            criteria.RoleId = 1;
            criteria.CurrencyId = 16;
            criteria.CurrencyName = "Yen";
            criteria.SubscriberId = 169;
            criteria.DeviceId = 59671118;
            criteria.CreatedDate = DateTime.Now.ToString();
            queue.Enqueue(criteria);

            return null;
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

        public static void RunQueue(object o)
        {
            while (true)
            {
                try
                {
                    AddCustomerAndAssociationCriteria criteria;
                    queue.TryDequeue(out criteria);

                    if (criteria != null)
                    {
                        InsertCustomerAndAssociation(criteria);
                    }
                }
                catch (Exception ex)
                {
                    LogService.Info(ex.Message, ex);
                }
            }
        }

        static FPSController()
        {
            thread = new Thread(RunQueue);

            thread.Start();
        }

        private static void InsertCustomerAndAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            var groupIds = DbSessionFactory.Execute<int>(FPS.AddCustomerAndAssociation, criteria);
            var mergedGroupId = DbSessionFactory.ExecuteSingle<int>(FPS.MergeGroup, new { groupIds });
            DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, GroupId = mergedGroupId });
        }
    }
}