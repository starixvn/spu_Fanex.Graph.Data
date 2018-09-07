using System.Collections.Generic;

namespace GraphAPI.Criteria
{
    public class MergeExistingGroupCriteria
    {
        public IEnumerable<int> GroupIdList { get; set; }

        public int CustId { get; set; }
    }
}