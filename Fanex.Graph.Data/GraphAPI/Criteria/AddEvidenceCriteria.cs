using System.Collections.Generic;

namespace GraphAPI.Criteria
{
    public class AddEvidenceCriteria
    {
        public int CustId { get; set; }

        public IEnumerable<string> EvidenceCodes { get; set; }

        public string CreatedDate { get; set; }
    }
}