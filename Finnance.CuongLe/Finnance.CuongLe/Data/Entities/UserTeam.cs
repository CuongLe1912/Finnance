using Finnance.CuongLe.Data.Entities.Account;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities
{
    public partial class UserTeam : BaseEntity
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }

        [IgnoreDataMember]
        public virtual User? User { get; set; }

        [IgnoreDataMember]
        public virtual Team? Team { get; set; }

    }
}
