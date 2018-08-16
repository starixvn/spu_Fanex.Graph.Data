namespace Fanex.Graph.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Xml.Linq;

    public class DbConnectionConfiguration
    {
        private DbConnectionConfiguration()
        {
        }

        internal static IDictionary<string, object> UriDict { get; } = new Dictionary<string, object>();

        internal static IDictionary<string, GraphSetting> SpDict { get; } = new Dictionary<string, GraphSetting>();

        private static DbConnectionConfiguration instance;

        //public DbConnectionConfiguration RegisterUris(params string[] configKeys)
        //{
        //    foreach (var key in configKeys)
        //    {
        //        var value = ConfigurationManager.AppSettings[key];

        //        if (value is null)
        //        {
        //            throw new InvalidOperationException($"Key {key} doesn't exist");
        //        }

        //        var rawSettings = value.Split(',');

        //        var settings = new DbSettings
        //        {
        //            Uri = rawSettings[0],
        //            User = rawSettings[1],
        //            Password = rawSettings[2]
        //        };

        //        UriDict.Add(key, settings);
        //    }

        //    return this;
        //}

        public DbConnectionConfiguration RegisterXml(string xmlDir)
        {
            var files = Directory.GetFiles(xmlDir, "*.xml");

            foreach (var xmlFile in files)
            {
                var document = XDocument.Load(xmlFile);

                foreach (var setting in document.Elements("GraphSetting"))
                {
                    var key = setting.Attribute("Key").Value;
                    var spName = setting.Attribute("SpName").Value;
                    var db = setting.Attribute("DbName").Value;

                    if (key is null)
                    {
                        throw new InvalidOperationException($"There's no Key attribute for {setting}");
                    }

                    if (spName is null)
                    {
                        throw new InvalidOperationException($"There's no SpName attribute for {setting}");
                    }

                    if (db is null)
                    {
                        throw new InvalidOperationException($"There's no DbName attribute for {setting}");
                    }

                    SpDict.Add(setting.Attribute("Key").Value, new GraphSetting
                    {
                        DbKey = db,
                        SpKey = key,
                        SpName = spName
                    });
                }
            }

            return this;
        }

        public static DbConnectionConfiguration Init() => instance = instance ?? new DbConnectionConfiguration();
    }
}