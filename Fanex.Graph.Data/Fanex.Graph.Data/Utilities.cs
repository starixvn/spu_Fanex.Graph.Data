using System.IO;
using System.Linq;
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

                var result = file.Directory.Parent.GetDirectories(path.Replace("~/", ""))[0];

                return result.FullName;
            }

            return path;
        }
    }
}