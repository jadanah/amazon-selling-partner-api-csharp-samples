using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.Finances.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpFinancesTests
    {
        [Fact]
        public async Task Should_get_financial_events()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerFinancesClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.ListFinancialEventsAsync(10, postedAfter: new DateTime(2022, 03, 01));

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_financial_event_groups()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerFinancesClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.ListFinancialEventGroupsAsync(10, financialEventGroupStartedAfter: new DateTime(2022, 03, 01));

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_financial_events_by_order_id()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerFinancesClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var response = await client.ListFinancialEventsByOrderIdAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }
    }
}