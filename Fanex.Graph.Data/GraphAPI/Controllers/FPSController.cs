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
                        DbSessionFactory.Execute(FPS.AddCustomerAndAssociation, criteria);
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
    }
}