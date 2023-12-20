namespace Finnance.CuongLe.Data.Entities
{
    public class LogActivity : BaseEntity
    {
        public string? Ip { get; set; }
        public string? Url { get; set; }
        public int? UserId { get; set; }
        public string? Body { get; set; }
        public string? Notes { get; set; }
        public string? Method { get; set; }
        public string? Action { get; set; }
        public string? ObjectId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Controller { get; set; }
    }
}
