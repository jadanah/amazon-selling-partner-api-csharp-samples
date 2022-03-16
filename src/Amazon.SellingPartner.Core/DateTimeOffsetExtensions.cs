using System;

namespace Amazon.SellingPartner.Core
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToAmazonDateTimeOffsetString(this DateTimeOffset value) => AmazonDateUtil.ConvertToString(value);
    }
}