using Finnance.CuongLe.Data.Entities.Catalog;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class UserDistrict : BaseEntity
    {
        public int UserId { get; set; }
        public bool? Allow { get; set; }
        public int DistrictId { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }

        [IgnoreDataMember]
        public virtual District District { get; set; }
    }
}
