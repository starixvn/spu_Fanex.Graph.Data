using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Graph.Data.Models
{
    public class CustomerGroup
    {
        public IEnumerable<int> DeviceGroups { get; set; }

        public IEnumerable<int> CustomerGroups { get; set; }
    }
}
