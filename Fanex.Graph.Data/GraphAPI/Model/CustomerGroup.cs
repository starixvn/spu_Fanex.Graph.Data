using System.Collections.Generic;

namespace GraphAPI.Model
{
    public class MergingGroups
    {
        public IEnumerable<int> DeviceGroups { get; set; }

        public IEnumerable<int> CustomerGroups { get; set; }
    }
}