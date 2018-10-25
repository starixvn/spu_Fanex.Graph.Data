using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GraphAPI.Helper
{
    public static class ObjectHelper
    {
        public static IDictionary<string, object> ToHashTable(this object o)
        {
            var json = JObject.FromObject(o);

            return json.ToObject<IDictionary<string, object>>();
        }

        public static string FormatJson(this object o)
        {
            var json = JObject.FromObject(o);

            return json.ToString();
        }
    }
}