using System;

namespace URF.Core.Helper.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToTimeStamp(this DateTime source)
        {
            return new DateTimeOffset(source).ToUnixTimeSeconds();
        }

        public static long ToTimeStamp(this DateTime? source)
        {
            return source == null ? 0 : source.Value.ToTimeStamp();
        }

        public static DateTime ToDateTime(this long unixTimeStamp)
        {
            if (unixTimeStamp.ToString().Length == 10) unixTimeStamp *= 1000;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Unspecified);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
