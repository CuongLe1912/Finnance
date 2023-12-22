using Finnance.CuongLe.Data.Entities.Account;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Catalog
{
    public partial class Country : BaseEntity
    {
        public string? Name { get; set; }
        public string? MeeyId { get; set; }
        public string? SystemName { get; set; }
        public string? DialingCode { get; set; }

        [IgnoreDataMember]
        public virtual List<User>? Users { get; set; }

        [IgnoreDataMember]
        public virtual List<City>? Cities { get; set; }
    }
}
