using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Catalog
{
    public partial class Ward : BaseEntity
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? MeeyId { get; set; }
        public int DistrictId { get; set; }
        public string? EnglishName { get; set; }

        [IgnoreDataMember]
        public virtual District? District { get; set; }
    }
}
