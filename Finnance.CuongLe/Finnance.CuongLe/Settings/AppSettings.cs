using Finnance.CuongLe.Data.Enums;

namespace Finnance.CuongLe.Settings
{
    public class AppSettings
    {
        public bool? InitDb { get; set; }
        public string Secret { get; set; }
        public string DbType { get; set; }
        public ProductionType ProductionType { get; set; }
        public string SchemaApi { get; set; }
        public string SchemaWeb { get; set; }
        public string TokenKey { get; set; }
        public string GraphQLURI { get; set; }
        public string GraphQLUser { get; set; }
        public string GraphQLPass { get; set; }
        public static string AmazonKey = "AKIAUSM65HCWHWXNNZAO";
        public static string AmazonSecret = "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";

    }
}