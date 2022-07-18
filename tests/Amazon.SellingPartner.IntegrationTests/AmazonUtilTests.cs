using System;
using Amazon.SellingPartner.Core;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonUtilTests
    {
        [Fact]
        public void Should_parse_date_time_offset_string()
        {
            var value = "2022-03-01T00:00:00-07:00";
            var expected = new DateTimeOffset(2022, 03, 01, 00, 00, 00, TimeSpan.FromHours(-7));

            var actual = AmazonDateUtil.ConvertToDateTimeOffset(value);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Should_parse_date_time_string()
        {
            var value = "2022-03-04T00:00:00Z";
            var expected = new DateTime(2022, 03, 04, 00, 00, 00, DateTimeKind.Utc);

            var actual = AmazonDateUtil.ConvertToDateTime(value);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Should_convert_to_date_time_offset_string()
        {
            var value = new DateTimeOffset(2022, 03, 01, 00, 00, 00, TimeSpan.FromHours(-7));
            var expected = "2022-03-01T00:00:00-07:00";

            var actual = AmazonDateUtil.ConvertToString(value);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Should_convert_to_date_time_string()
        {
            var value = new DateTime(2022, 03, 04, 00, 00, 00, DateTimeKind.Utc);
            var expected = "2022-03-04T00:00:00Z";

            var actual = AmazonDateUtil.ConvertToString(value);

            actual.Should().Be(expected);
        }
    }
}