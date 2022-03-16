using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.Sales.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpSalesTests
    {
        [Fact]
        public async Task Should_get_sales_order_metrics()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerSalesClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            // "2022-03-01T00:00:00-07:00--2022-03-04700:00:00-07:00"
            // "2022-03-01T00:00:00Z--2022-03-04T00:00:00Z"
            var startDate = new DateTime(2022, 03, 01, 00, 00, 00, DateTimeKind.Utc);
            var endDate = new DateTime(2022, 03, 07, 00, 00, 00, DateTimeKind.Utc);

            var response = await client.GetOrderMetricsAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, AmazonDateUtil.ConvertToString(startDate, endDate), Granularity.Total);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }
    }
}