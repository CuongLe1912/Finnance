using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string Types { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int? OrganizationId { get; set; }

        [IgnoreDataMember]
        public virtual Organization Organization { get; set; }

        [IgnoreDataMember]
        public virtual List<LinkPermission> LinkPermissions { get; set; }

        [IgnoreDataMember]
        public virtual List<UserPermission> UserPermissions { get; set; }

        [IgnoreDataMember]
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
