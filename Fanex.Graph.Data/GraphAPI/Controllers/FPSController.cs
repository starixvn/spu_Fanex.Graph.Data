using Fanex.Graph.Data;
using Fanex.Logging;
using GraphAPI.Criteria;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static ConcurrentQueue<AddCustomerAndAssociationCriteria> q = new ConcurrentQueue<AddCustomerAndAssociationCriteria>();

        private static Thread thread;

        private static int batch = 2;

        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            criteria.CreatedDate = DateTime.Now.ToString();

            q.Enqueue(criteria);
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
                    var tsks = Enumerable.Range(0, batch).Select(_ =>
                    {
                        q.TryDequeue(out var criteria);

                        return criteria == null ? Task.CompletedTask : new Task(() => InsertCustomer(criteria));
                    }).ToArray();

                    Task.WaitAll(tsks);
                }
                catch (Exception ex)
                {
                    LogService.Info(ex.Message, ex);
                }
            }
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

        static FPSController()
        {
            thread = new Thread(RunQueue);

            thread.Start();
        }
    }
}