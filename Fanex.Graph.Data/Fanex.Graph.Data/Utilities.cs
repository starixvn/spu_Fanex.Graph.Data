using System.IO;
using System.Reflection;

namespace Fanex.Graph.Data
{
    public static class Utilities
    {
        public static string ResolvePath(string path)
        {
            if (path.StartsWith("~"))
            {
                var location = Assembly.GetExecutingAssembly().CodeBase;

                var file = new FileInfo(location.Replace("file:///", ""));

                var arr = path.Replace("~/", "").Split('/');

                var result = file.Directory.Parent;

                for (int i = 0; i < arr.Length; i++)
                {
                    result = result.GetDirectories(arr[i], SearchOption.AllDirectories)[0];
                }
               
                return result.FullName;
            }

            return path;
        }
    }
}