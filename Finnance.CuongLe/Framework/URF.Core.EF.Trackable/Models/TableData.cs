using System.Collections.Generic;

namespace URF.Core.EF.Trackable.Models
{
    public class TableData
    {
        public string Name { get; set; }
        public string Search { get; set; }
        public PagingData Paging { get; set; }
        public ExportData Export { get; set; }
        public List<int> IgnoreIds { get; set; }
        public List<string> Columns { get; set; }
        public List<OrderData> Orders { get; set; }
        public List<FilterData> Filters { get; set; }
    }
}
