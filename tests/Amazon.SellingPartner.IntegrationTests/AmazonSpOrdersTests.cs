using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.Orders.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpOrdersTests
    {
        [Fact]
        public async Task Should_get_order_by_id()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetOrderAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_order_items_by_id()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetOrderItemsAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_orders_by_date()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: "2022-03-07T00:00:00Z");

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }
    }
}