using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPI.Models
{
    public class Association
    {
        public int DeviceId { get; set; }

        public int FpsAccountId { get; set; }

        public string FpsUsername { get; set; }
    }
}