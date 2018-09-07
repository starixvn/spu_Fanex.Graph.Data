using System;

namespace GraphAPI.Criteria
{
    public class AddCustomerAndAssociationCriteria
    {
        public int DeviceId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; } = "harvey";

        public string Username2 { get; set; } = "username2";

        public int RoleId { get; set; } = 0;

        public int CurrencyId { get; set; } = 0;

        public string CurrencyName { get; set; } = "NVD";

        public int SubscriberId { get; set; } = 0;

        public int FpsAccountId { get; set; } = 0;

        public int IsDeposit { get; set; } = 0;
    }
}