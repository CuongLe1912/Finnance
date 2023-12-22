using Finnance.CuongLe.Data.Entities.Account;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Finnance.CuongLe.Data.Entities.Catalog
{
    public partial class District : BaseEntity
    {
        public int? CityId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? MeeyId { get; set; }
        public string? EnglishName { get; set; }

        [IgnoreDataMember]
        public virtual City? City { get; set; }

        [IgnoreDataMember]
        public virtual List<Ward>? Wards { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<Street>? Streets { get; set; }

        [IgnoreDataMember]
        public virtual List<UserDistrict>? UserDistricts { get; set; }

    }
}
