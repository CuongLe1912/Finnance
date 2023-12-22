using Finnance.CuongLe.Data.Enums;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public bool? Allow { get; set; }
        public int PermissionId { get; set; }
        public PermissionType Type { get; set; }

        [IgnoreDataMember]
        public virtual Role Role { get; set; }

        [IgnoreDataMember]
        public virtual Permission Permission { get; set; }
    }
}
