using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;
using URF.Core.EF.Trackable.Enums;

namespace Finnance.CuongLe.Data.Entities.Account
{
    public partial class User : IdentityUser<int>, ITrackable, IMergeable, ISqlExEntity
    {
        public bool? Locked { get; set; }

        [StringLength(255)]
        public string? Avatar { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public string? Address { get; set; }

        public string? FullName { get; set; }

        public int? PositionId { get; set; }

        public int? DepartmentId { get; set; }

        public string? VerifyCode { get; set; }

        public string? ReasonLock { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Description { get; set; }

        public GenderType? Gender { get; set; }

        public int? ExtPhoneNumber { get; set; }

        public bool NeedOTP { get; set; }
        public string? OTPSecretKey { get; set; }
        public string? OTPEncodedKey { get; set; }
        public string? OTPQrCodeImage { get; set; }

        public DateTime? VerifyTime { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [IgnoreDataMember]
        [ForeignKey("CreatedBy")]
        public virtual User? CreatedByUser { get; set; }

        [IgnoreDataMember]
        [ForeignKey("UpdatedBy")]
        public virtual User? UpdatedByUser { get; set; }

        [IgnoreDataMember]
        public virtual List<UserRole>? UserRoles { get; set; }

        [IgnoreDataMember]
        public virtual List<UserActivity>? Activities { get; set; }

        [IgnoreDataMember]
        public virtual List<LogActivity>? LogActivities { get; set; }

        [IgnoreDataMember]
        public virtual List<LogException>? LogExceptions { get; set; }

        [IgnoreDataMember]
        public virtual List<UserPermission>? UserPermissions { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public Guid EntityIdentifier { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public TrackingState TrackingState { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public ICollection<string>? ModifiedProperties { get; set; }
    }
}
