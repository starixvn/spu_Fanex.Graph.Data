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

            var merger = DbSessionFactory.QuerySingle<MergingGroups>(FPS.AddCustomerAndAssociation, criteria);

            if (merger.CustomerGroups.Any())
            {
                var cusGroup = merger.CustomerGroups.First();

                if (merger.DeviceGroups.Any())
                {
                    DbSessionFactory.Execute(FPS.MarkMergeGroup, new { CustGroupId = cusGroup, DevGroupIds = merger.DeviceGroups });
                }
            }
            else
            {
                if (merger.DeviceGroups.Any())
                {
                    var devGroup = merger.DeviceGroups.First();

                    DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, GroupId = devGroup });
                }
                else
                {
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