using log4net;
using System;

namespace GraphAPI.Factory
{
    public static class Logger
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Error(string msg, Exception ex) => logger.Error(msg, ex);

        public static void Info(string msg) => logger.Info(msg);
    }
}