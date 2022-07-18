using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.FulfillmentInbound.Client;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpFulfillmentInboundTests
    {
        private readonly IAmazonSellingPartnerFulfillmentInboundClient _client;

        public AmazonSpFulfillmentInboundTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateFulfillmentInboundClient();
        }

        [Fact(Skip = "Model doesnt look right as v0")]
        public async Task Should_get_fba_inbound_shipments()
        {
            var response = await _client.GetShipmentsAsync(QueryType.DATE_RANGE, marketplaceId: AmazonMarketplace.UK.MarketplaceId, lastUpdatedAfter: new DateTime(2022, 03, 01),
                lastUpdatedBefore: new DateTime(2022, 03, 07));

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_fba_shipment_items_by_id()
        {
            var response = await _client.GetShipmentItemsByShipmentIdAsync("FBA15FQSDSV5", AmazonMarketplace.UK.MarketplaceId);

            response.Should().NotBeNull();
        }
    }
}