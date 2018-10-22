using EasyNetQ;
using EasyNetQ.Topology;
using Fanex.Graph.Data;
using Fanex.Graph.Data.Models.Core;
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
        private static IAdvancedBus _advancedBus;

        static FPSController()
        {
            var connectionString = string.Format(
                "host={0};virtualHost={1};prefetchcount={2};username={3};password={4}",
                AppSetting.Host,
                AppSetting.VirtualHost,
                AppSetting.PrefetchCount,
                AppSetting.Username,
                AppSetting.Password);

            _advancedBus = RabbitHutch.CreateBus(connectionString).Advanced;
            var _queue = _advancedBus.QueueDeclare(AppSetting.QueueName);
            var _exchange = _advancedBus.ExchangeDeclare(AppSetting.ExchangeName, ExchangeType.Direct);
            _advancedBus.Bind(_exchange, _queue, AppSetting.RoutingKey);

            try
            {
                _advancedBus.Consume(_queue,
                    registration => registration.Add<AddCustomerAndAssociationCriteria>((message, info) =>
                    {
                        InsertCustomer(message.Body);
                    }));
            }
            catch (Exception ex)
            {
                LogService.Info(ex.Message, ex);
            }
        }

        public static void GetMessageFromQueue(object o)
        {
           
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

            var _queue = _advancedBus.QueueDeclare(AppSetting.QueueName);
            var _exchange = _advancedBus.ExchangeDeclare(AppSetting.ExchangeName, ExchangeType.Direct);
            _advancedBus.Bind(_exchange, _queue, AppSetting.RoutingKey);

            var message = new Message<AddCustomerAndAssociationCriteria>(criteria);
            _advancedBus.PublishAsync(_exchange, AppSetting.RoutingKey, true, message);
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