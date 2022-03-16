using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Amazon.SellingPartner.Reports.Client;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpReportTests
    {
        [Fact]
        public async Task Should_get_report_end_to_end()
        {
            var httpClient = new TestAmazonSpHttpClientFactory().Create();
            var client = new AmazonSellingPartnerReportsClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var createResponse = await client.CreateReportAsync(new CreateReportSpecification()
            {
                MarketplaceIds = new List<string>() { AmazonMarketplace.UK.MarketplaceId },
                ReportType = "GET_V2_SELLER_PERFORMANCE_REPORT",
            });

            var reportId = createResponse.ReportId;

            ReportProcessingStatus status;
            string reportDocumentId;
            do
            {
                await Task.Delay(10000);
                var getResponse = await client.GetReportAsync(reportId);
                status = getResponse.ProcessingStatus;
                reportDocumentId = getResponse.ReportDocumentId;
            } while (status != ReportProcessingStatus.DONE);

            var documentResponse = await client.GetReportDocumentAsync(reportDocumentId);
            var downloadUrl = documentResponse.Url;

            downloadUrl.Should().NotBeNull();
        }
    }
}