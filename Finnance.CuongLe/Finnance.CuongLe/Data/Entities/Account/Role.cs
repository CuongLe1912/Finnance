using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class Role : IdentityRole<int>, ITrackable, IMergeable, ISqlExEntity
    {
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [IgnoreDataMember]
        public virtual Organization Organization { get; set; }

        [IgnoreDataMember]
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [IgnoreDataMember]
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }

        [IgnoreDataMember]
        public virtual List<UserRole> UserRoles { get; set; }

        [IgnoreDataMember]
        public virtual List<RolePermission> RolePermissions { get; set; }

        [NotMapped]
        public Guid EntityIdentifier { get; set; }

        [NotMapped]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }
    }
}
