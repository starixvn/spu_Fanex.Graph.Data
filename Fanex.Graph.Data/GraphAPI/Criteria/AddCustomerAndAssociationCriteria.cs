using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPI.Criteria
{
    public class AddCustomerAndAssociationCriteria
    {
        public int DeviceId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public int RoleId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int SubscriberId { get; set; }

        public int FpsAccountId { get; set; }

        public int IsDeposit { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}