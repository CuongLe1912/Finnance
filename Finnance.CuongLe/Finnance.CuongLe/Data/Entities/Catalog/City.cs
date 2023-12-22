using Finnance.CuongLe.Data.Entities.Catalog;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Finnance.CuongLe.Data.Entities.Catalog
{
    public partial class City : BaseEntity
    {
        public string Name { get; set; }
        public string MeeyId { get; set; }
        public int? CountryId { get; set; }
        public string Title { get; set; }
        public string EnglishName { get; set; }
        public int? Order { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Country Country { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<Street> Streets { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<District> Districts { get; set; }

    }
}
