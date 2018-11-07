using log4net;
using Newtonsoft.Json.Linq;
using System;

namespace GraphBackgroundService
{
    public static class LogFactory
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(Exception ex, string message = null)
        {
            logger.Error(message ?? ex.Message, ex);
        }

       public static void LogInfo(string info)
        {
            logger.Info(info);
        }

        public static void LogInfo(object o)
        {
            var json = JObject.FromObject(o).ToString();

            LogInfo(json);
        }
    }
}