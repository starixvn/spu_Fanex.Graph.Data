using Fanex.Graph.Data;
using Fanex.Logging;
using GraphAPI.Criteria;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static ConcurrentQueue<AddCustomerAndAssociationCriteria> queue = new ConcurrentQueue<AddCustomerAndAssociationCriteria>();

        private static Thread thread;

        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
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

        public static void RunQueue(object o)
        {
            while (true)
            {
                try
                {
                    queue.TryDequeue(out var criteria);

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
            var groupIds = DbSessionFactory.Query<int>(FPS.AddCustomerAndAssociation, criteria).ToArray();
            var mergedGroupId = DbSessionFactory.QuerySingle<int>(FPS.MergeGroup, new { groupIds });
            DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, GroupId = mergedGroupId });
        }
    }
}