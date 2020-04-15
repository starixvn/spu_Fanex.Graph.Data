using Fanex.Graph.Data;
using GraphAPI.Criteria;
using GraphAPI.Factory;
using GraphAPI.Helper;
using GraphAPI.Models;
using GraphAPI.Service.Implement.FPS;
using GraphAPI.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace GraphAPI.Controllers
{
    public class FPSController : ApiController
    {
        private static IAsyncQueue<AddCustomerAndAssociationCriteria> queue;

        static FPSController()
        {
            try
            {
                queue = new EasyNetQService();
                if (Convert.ToBoolean(AppSettings.IsTurnOnQueueHandling))
                {
                    queue.AddHandler(InsertCustomer);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        [HttpPost]
        public void AddAssociation(AddCustomerAndAssociationCriteria criteria)
        {
#if DEBUG
            criteria.CustId = 32297449;
            criteria.Username = "BV890988888";
            criteria.Username2 = null;
            criteria.RoleId = 1;
            criteria.DeviceId = 59671118; 
#endif
            if (criteria.RoleId == 1)
            {
                queue.Enqueue(criteria);
            }
        }

        // may bi ung thu => Sua cho nay
        [HttpPost]
        public object AddEvidence(AddEvidenceCriteria criteria)
        {
            //Logger.Info(criteria.FormatJson());
            //criteria.CreatedDate = DateTime.Now.ToString();
            //return DbSessionFactory.Execute(FPS.AddCustomerAndEvidence, criteria);
            return null;
        }

        [HttpPost]
        public object RemoveEvidenceByCustomerList(RemoveEvidenceByCustomerListCriteria criteria)
        {
            //return DbSessionFactory.Execute(FPS.RemoveEvidenceByCustomerList, criteria);
            return null;

        }

        [HttpPost]
        public object UpdateUsername2(UpdateUsername2Criteria criteria)
        {
            return DbSessionFactory.Execute(FPS.UpdateCustomerUsername2, criteria);
        }

        [AcceptVerbs(WebRequestMethods.Http.Get, WebRequestMethods.Http.Post)]
        public IHttpActionResult GetAssociationLevel2([FromBody] string custIds)
        {
            var custIdList = custIds.Split(',').Select(a => Convert.ToInt32(a));
            IEnumerable<CustomerAssociationLevel2> result = null;
            try
            {
                result = DbSessionFactory.Execute<CustomerAssociationLevel2>(FPS.GetAssociationLevel2, new { CustIdList = custIdList }).Data;
            }
            catch (Exception ex)
            {
                Logger.Error($"GetAssociationLevel2 - {ex.Message}", ex);
            }

            if (result == null)
            {
                Logger.Error($"GetAssociationLevel2 - Result is null !");
            }

            return Json(result);
        }

        [AcceptVerbs(WebRequestMethods.Http.Get, WebRequestMethods.Http.Post)]
        public IHttpActionResult GetAssociationLevel2Test([FromBody] string custIds)
        {
            var custIdList = custIds.Split(',').Select(a => Convert.ToInt32(a));
            List<CustomerAssociationLevel2> result = null;
            try
            {
                result = new List<CustomerAssociationLevel2>();
                int[] listAss = { 1, 2, 3 };
                result.Add(new CustomerAssociationLevel2()
                {
                    CustId = 1,
                    Level2AssociatedCustIds = listAss
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"GetAssociationLevel2 - {ex.Message}", ex);
            }

            if (result == null)
            {
                Logger.Error($"GetAssociationLevel2 - Result is null !");
            }
            return Json(result);
        }

        private static void InsertCustomer(AddCustomerAndAssociationCriteria criteria)
        {
            // ko luu SMA, chi luu member, roleid = 1
            if (criteria.RoleId == 1)
            {
                // remove test account
                if (!criteria.Username.Contains("test") || criteria.Username2 == null || !criteria.Username2.Contains("test"))
                {
                    try
                    {
                        var custId = DbSessionFactory.Query<int>(FPS.GetCustomerById, new { criteria.CustId });
                        var deviceId = DbSessionFactory.Query<int>(FPS.GetDeviceById, new { criteria.DeviceId });
                        var association = DbSessionFactory.Query<int>(FPS.CheckExistedAssociation, new { criteria.CustId, criteria.DeviceId });
                        if (criteria.DeviceId == 0)
                        {
                            if (!custId.Any())
                            {
                                DbSessionFactory.Execute(FPS.AddNewCustomer, criteria);
                            }
                        }
                        else
                        {
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
                                        DbSessionFactory.Execute(FPS.MergeGroup, new { criteria.CustId, criteria.DeviceId });
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
                    catch (Exception ex)
                    {
                        Logger.Error($"{ex.Message}\n{criteria.FormatJson()}", ex);
                    }
                }
            }
        }
    }
}