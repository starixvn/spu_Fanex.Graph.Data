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
                var location = Assembly.GetExecutingAssembly().Location;

                var file = new FileInfo(location);

                return path.Replace("~", file.Directory.FullName);
            }

            return path;
        }
    }
}