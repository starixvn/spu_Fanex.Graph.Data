  
  
 


namespace GraphBackgroundService
{
    using System.Configuration;

    public static class AppSettings
    {
                public static string DB_MiddleService => ConfigurationManager.AppSettings["DB.MiddleService"];
            }
}

