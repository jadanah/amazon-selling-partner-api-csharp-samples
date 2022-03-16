using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.ProductFees.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpProductFeesTests
    {
        [Fact]
        public async Task Should_get_fee_estimate_by_sku()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerProductFeesClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.GetMyFeesEstimateForSKUAsync(new GetMyFeesEstimateRequest()
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