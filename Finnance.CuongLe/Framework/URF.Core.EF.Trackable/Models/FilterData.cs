using URF.Core.EF.Trackable.Enums;

namespace URF.Core.EF.Trackable.Models
{
    public class FilterData
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public CompareType Compare { get; set; }
    }
}
