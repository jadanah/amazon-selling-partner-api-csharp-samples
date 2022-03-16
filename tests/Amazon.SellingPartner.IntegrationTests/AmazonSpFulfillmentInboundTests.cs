using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.FulfillmentInbound.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpFulfillmentInboundTests
    {
        [Fact(Skip = "Model doesnt look right as v0")]
        public async Task Should_get_fba_inbound_shipments()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerFulfillmentInboundClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetShipmentsAsync(QueryType.DATE_RANGE, marketplaceId: AmazonMarketplace.UK.MarketplaceId, lastUpdatedAfter: new DateTime(2022, 03, 01),
                lastUpdatedBefore: new DateTime(2022, 03, 07));

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_fba_shipment_items_by_id()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerFulfillmentInboundClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetShipmentItemsByShipmentIdAsync("FBA15FQSDSV5", AmazonMarketplace.UK.MarketplaceId);

            response.Should().NotBeNull();
        }
    }
}