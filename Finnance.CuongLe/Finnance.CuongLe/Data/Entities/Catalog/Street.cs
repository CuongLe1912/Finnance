using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Finnance.CuongLe.Data.Entities.Catalog
{
    public partial class Street : BaseEntity
    {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? MeeyId { get; set; }
        public int DistrictId { get; set; }
        public string? EnglishName { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual City? City { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual District? District { get; set; }
    }
}
