using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public class LinkPermission : BaseEntity
    {
        public int? Order { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Group { get; set; }
        public int? ParentId { get; set; }
        public string CssIcon { get; set; }
        public int? GroupOrder { get; set; }
		public int? PermissionId { get; set; }

        [IgnoreDataMember]
        public virtual Permission Permission { get; set; }

        [IgnoreDataMember]
        public virtual LinkPermission Parent { get; set; }
    }
}
