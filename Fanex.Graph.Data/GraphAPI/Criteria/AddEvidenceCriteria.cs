using System.Collections.Generic;

namespace GraphAPI.Criteria
{
    public class AddEvidenceCriteria
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public IEnumerable<string> EvidenceCodesForGraph { get; set; }

        public int RoleId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int SubscriberId { get; set; }

        public int FpsAccountId { get; set; }

        public bool IsDeposit { get; set; }

        public string CreatedDate { get; set; }
    }
}