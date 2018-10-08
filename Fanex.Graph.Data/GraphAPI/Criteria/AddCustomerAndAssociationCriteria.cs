using System;

namespace GraphAPI.Criteria
{
    public class AddCustomerAndAssociationCriteria
    {
        private string username2;
        private int? roleId;
        private int? currencyId;
        private string currencyName;

        public int DeviceId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2
        {
            get => username2 == string.Empty ? null : username2;
            set { username2 = value; }
        }

        public int? RoleId
        {
            get => roleId == 0 ? null : roleId;

            set { roleId = value; }
        }

        public int? CurrencyId
        {
            get => currencyId == 0 ? null : currencyId;

            set { currencyId = value; }
        }

        public string CurrencyName
        {
            get => currencyName == string.Empty ? null : currencyName;
            set { currencyName = value; }
        }

        public int SubscriberId { get; set; }

        public int FpsAccountId { get; set; }

        public bool IsDeposit { get; set; }

        public string CreatedDate { get; set; }
    }
}