using System;

namespace Amazon.SellingPartner.Core
{
    public static class DateTimeExtensions
    {
        public static string ToAmazonDateTimeString(this DateTime value) => AmazonDateUtil.ConvertToString(value);
    }
}