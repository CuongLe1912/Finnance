using System;
using URF.Core.EF.Trackable.Enums;

namespace URF.Core.EF.Trackable.Models
{
    public class ExportData
    {
        public int Limit { get; set; }
        public bool Landscape { get; set; }
        public ExportType Type { get; set; }
        public DateTime[] DateRange { get; set; }
        public PdfPageSizeType PageSize { get; set; }
    }
}
