using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Finances.Client;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpFinancesTests
    {
        private readonly IAmazonSellingPartnerFinancesClient _client;

        public AmazonSpFinancesTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateFinancesClient();
        }

        [Fact]
        public async Task Should_get_financial_events()
        {
            var startDate = new DateTime(2022, 03, 01);
            var endDate = new DateTime(2022, 03, 03);

            var response = await _client.ListFinancialEventsAsync(100, postedAfter: startDate, postedBefore: endDate);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();

            var nextToken = response.Payload.NextToken;
            while (!string.IsNullOrWhiteSpace(nextToken))
            {
                var nextResponse = await _client.ListFinancialEventsAsync(nextToken: nextToken);

                nextResponse.Should().NotBeNull();
                nextResponse.Payload.Should().NotBeNull();

                nextToken = nextResponse.Payload.NextToken;
            }
        }

        [Fact]
        public async Task Should_get_financial_event_groups()
        {
            var startDate = new DateTime(2022, 03, 01);
            var endDate = new DateTime(2022, 03, 03);

            var response = await _client.ListFinancialEventGroupsAsync(100, financialEventGroupStartedAfter: startDate, financialEventGroupStartedBefore: endDate);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();

            var nextToken = response.Payload.NextToken;
            while (!string.IsNullOrWhiteSpace(nextToken))
            {
                var nextResponse = await _client.ListFinancialEventGroupsAsync(nextToken: nextToken);

                nextResponse.Should().NotBeNull();
                nextResponse.Payload.Should().NotBeNull();

                nextToken = nextResponse.Payload.NextToken;
            }
        }

        [Fact]
        public async Task Should_get_financial_events_by_order_id()
        {
            var orderId = "026-3243799-6137111";

            var response = await _client.ListFinancialEventsByOrderIdAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
        }
    }
}