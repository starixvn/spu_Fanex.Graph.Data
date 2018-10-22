using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Fanex.Graph.Data.Models.Core
{
    public static class AppSetting
    {
        public static string QueueName
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-Queue"];
            }
        }

        public static string ExchangeName
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-Exchange"];
            }
        }

        public static string RoutingKey
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-RoutingKey"];
            }
        }

        public static int Port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQ-Port"]);
            }
        }

        public static string VirtualHost
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-VirtualHost"];
            }
        }

        public static string Host
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-Host"];
            }
        }

        public static string PrefetchCount
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-PrefetchCount"];
            }
        }

        public static string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-Username"];
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMQ-Password"] ?? "admin";
            }
        }
    }
}
