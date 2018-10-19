using Fanex.Graph.Data;
using Fanex.Logging;
using GraphAPI.Criteria;
using GraphAPI.Model;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static Thread thread;

        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
            criteria.CreatedDate = DateTime.Now.ToString();
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
                else
                {
                    // case acc cu, device moi
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
                    DbSessionFactory.Execute(Jobs.MergeGroup);
                }
                catch (Exception ex)
                {
                    LogService.Info(ex.Message, ex);
                }
            }
        }

        static FPSController()
        {
            //thread = new Thread(RunQueue);

            //thread.Start();
        }
    }
}