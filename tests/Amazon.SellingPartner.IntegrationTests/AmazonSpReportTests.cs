using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using Amazon.SellingPartner.Reports.Client;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpReportTests
    {
        private readonly IAmazonSellingPartnerReportsClient _client;

        public AmazonSpReportTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateReportsClient();
        }

        [Fact]
        public async Task Should_get_seller_performance_report_end_to_end()
        {
            var reportType = "GET_V2_SELLER_PERFORMANCE_REPORT";
            var marketplaceIds = new List<string>() { AmazonMarketplace.UK.MarketplaceId };

            await DownloadAndDecompressReportEndToEndAsync(marketplaceIds, reportType);
        }

        [Fact]
        public async Task Should_get_inventory_report_end_to_end()
        {
            var reportType = "GET_FLAT_FILE_OPEN_LISTINGS_DATA";
            var marketplaceIds = new List<string>() { AmazonMarketplace.UK.MarketplaceId };

            await DownloadAndDecompressReportEndToEndAsync(marketplaceIds, reportType);
        }

        private async Task DownloadAndDecompressReportEndToEndAsync(ICollection<string> marketplaceIds, string reportType)
        {
            var createResponse = await _client.CreateReportAsync(new CreateReportSpecification()
            {
                MarketplaceIds = marketplaceIds,
                ReportType = reportType,
            });

            var reportId = createResponse.ReportId;

            ReportProcessingStatus status;
            string reportDocumentId;
            do
            {
                await Task.Delay(15000);
                var getResponse = await _client.GetReportAsync(reportId);
                status = getResponse.ProcessingStatus;
                reportDocumentId = getResponse.ReportDocumentId;
            } while (status != ReportProcessingStatus.DONE);

            var documentResponse = await _client.GetReportDocumentAsync(reportDocumentId);
            var downloadUrl = documentResponse.Url;

            downloadUrl.Should().NotBeNull();

            var client = new HttpClient();
            var response = await client.GetAsync(downloadUrl);
            var filePath = Path.Combine(Path.GetTempPath(), reportType + (documentResponse.CompressionAlgorithm == ReportDocumentCompressionAlgorithm.GZIP ? ".gz" : ".report"));

            await using (var fs = new FileStream(filePath, FileMode.Create))
                await response.Content.CopyToAsync(fs);

            var bytes = await File.ReadAllBytesAsync(filePath);
            bytes.Should().NotBeNull();
            bytes.Should().NotBeEmpty();

            if (documentResponse.CompressionAlgorithm == ReportDocumentCompressionAlgorithm.GZIP)
                await GzipUtil.DecompressAsync(filePath);
        }
    }
}