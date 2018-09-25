using System.Collections.Generic;

namespace GraphAPI.Criteria
{
    public class RemoveEvidenceByCustomerListCriteria
    {
        public IEnumerable<int> CustIds { get; set; }

        public string EviCode { get; set; }
    }
}