using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string Leader { get; set; }
        public string Website { get; set; }
        public string LeaderPhone { get; set; }
        public string LeaderEmail { get; set; }

        [IgnoreDataMember]
        public virtual List<Role> Roles { get; set; }

        [IgnoreDataMember]
        public virtual List<Team> Teams { get; set; }

        [IgnoreDataMember]
        public virtual List<Permission> Permissions { get; set; }
    }
}
