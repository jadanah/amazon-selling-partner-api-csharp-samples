using System;
using System.Globalization;

namespace Amazon.SellingPartner.Core
{
    public static class AmazonDateUtil
    {
        public static DateTime ConvertToDateTime(string value) => DateTime.Parse(value, null, DateTimeStyles.RoundtripKind);

        public static DateTimeOffset ConvertToDateTimeOffset(string value) => DateTimeOffset.Parse(value, null, DateTimeStyles.RoundtripKind);

        public static string ConvertToString(DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ssZ");

        public static string ConvertToString() => ConvertToString(new DateTimeOffset());

        public static string ConvertToString(DateTimeOffset value) => value.ToString("yyyy-MM-ddTHH:mm:sszzz");

        public static string ConvertToString(DateTime start, DateTime end, string delimiter = "--") => ConvertToString(start) + delimiter + ConvertToString(end);

        public static string ConvertToString(DateTimeOffset start, DateTimeOffset end, string delimiter = "--") => ConvertToString(start) + delimiter + ConvertToString(end);
    }
}