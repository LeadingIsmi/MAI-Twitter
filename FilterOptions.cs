using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter
{
    public class FilterOptions
    {
        public string SearchTerm { get; set; }
        public Dictionary<string, object> FilterCriteria { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
