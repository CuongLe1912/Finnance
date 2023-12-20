using Finnance.CuongLe.Data.Entities;

namespace Meey.Admin.Data.Entities
{
    public class LogException : BaseEntity
    {
        public int? UserId { get; set; }
        public string? Exception { get; set; }
        public DateTime DateTime { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }

        // virtual
        //public virtual User User { get; set; }
    }
}
