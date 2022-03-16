using System;

namespace Amazon.SellingPartner.Core
{
    public static class StringExtensions
    {
        public static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/"))
                return url + "/";

            return url;
        }

        public static DateTimeOffset ToDateTimeOffset(this string value) => AmazonDateUtil.ConvertToDateTimeOffset(value);

        public static DateTime ToDateTime(this string value) => AmazonDateUtil.ConvertToDateTime(value);
    }
}