using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using Amazon.SellingPartner.Orders.Client;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpOrdersTests
    {
        private readonly IAmazonSellingPartnerOrdersClient _client;

        public AmazonSpOrdersTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateOrdersClient();
        }

        [Fact]
        public async Task Should_get_order_by_id()
        {
            var orderId = "026-3243799-6137111";

            var response = await _client.GetOrderAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_order_items_by_id()
        {
            var orderId = "026-3243799-6137111";

            var response = await _client.GetOrderItemsAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_orders_by_date()
        {
            var startDate = new DateTime(2022, 03, 01, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(2022, 03, 07, 0, 0, 0, DateTimeKind.Utc);
            var marketplaceIds = new[] { AmazonMarketplace.UK.MarketplaceId };

            var response = await _client.GetOrdersAsync(marketplaceIds, createdAfter: startDate.ToAmazonDateTimeString(),
                createdBefore: endDate.ToAmazonDateTimeString());

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();

            var nextToken = response.Payload.NextToken;
            while (!string.IsNullOrWhiteSpace(nextToken))
            {
                var nextResponse = await _client.GetOrdersAsync(marketplaceIds, nextToken: nextToken);

                nextResponse.Should().NotBeNull();
                nextResponse.Payload.Should().NotBeNull();

                nextToken = nextResponse.Payload.NextToken;
            }
        }
    }
}