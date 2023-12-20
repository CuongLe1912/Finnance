namespace URF.Core.Helper.Extensions
{
    public static class BoolExtensions
    {
        public static bool HasValue(this bool value)
        {
            return value;
        }
        public static bool HasValue(this bool? value)
        {
            return value.HasValue && value.Value;
        }
    }
}