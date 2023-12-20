using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URF.Core.EF.Trackable
{
    public interface ISqlEntity
    {
        public int Id { get; set; }
    }

    public interface ISqlExEntity
    {
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class SqlEntity : Entity, ISqlEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public abstract class SqlExEntity : SqlEntity, ISqlExEntity
    {
        public SqlExEntity()
        {
            IsActive = true;
            IsDelete = false;
            CreatedDate = DateTime.Now;
        }

        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
