using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPI.Models
{
    public class CustomerAssociationLevel2
    {
        public int CustId { get; set; }

        public int[] AssociationLevel2 { get; set; }
    }
}