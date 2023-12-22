using System;
using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable.Enums;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class UserActivity : BaseEntity
    {
        [StringLength(25)]
        public string Ip { get; set; }

        [StringLength(255)]
        public string Os { get; set; }

        public int UserId { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(255)]
        public string Browser { get; set; }

        public bool Incognito { get; set; }

        public DateTime DateTime { get; set; }

        public UserActivityType Type { get; set; }

        // virtual
        public virtual User User { get; set; }
    }
}
