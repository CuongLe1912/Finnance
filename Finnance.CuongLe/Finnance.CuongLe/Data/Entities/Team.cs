using Finnance.CuongLe.Data.Entities.Account;
using System.Runtime.Serialization;

namespace Finnance.CuongLe.Data.Entities
{
    public class Team : BaseEntity
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? OrganizationId { get; set; }

        [IgnoreDataMember]
        public virtual List<UserTeam>? UserTeams { get; set; }

        [IgnoreDataMember]
        public virtual Organization? Organization { get; set; }

    }
}
