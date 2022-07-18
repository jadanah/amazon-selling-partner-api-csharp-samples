using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using Amazon.SellingPartner.ProductFees.Client;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpProductFeesTests
    {
        private readonly IAmazonSellingPartnerProductFeesClient _client;

        public AmazonSpProductFeesTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateProductFeesClient();
        }

        [Fact]
        public async Task Should_get_fee_estimate_by_sku()
        {
            var response = await _client.GetMyFeesEstimateForSKUAsync(new GetMyFeesEstimateRequest()
            {
                FeesEstimateRequest = new FeesEstimateRequest()
                {
                    Identifier = Guid.NewGuid().ToString(),
                    IsAmazonFulfilled = false,
                    MarketplaceId = AmazonMarketplace.UK.MarketplaceId,
                    PriceToEstimateFees = new PriceToEstimateFees()
                    {
                        ListingPrice = new MoneyType()
                        {
                            Amount = 14.99,
                            CurrencyCode = "GBP"
                        }
                    }
                }
            }, "5003");

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }
    }
}