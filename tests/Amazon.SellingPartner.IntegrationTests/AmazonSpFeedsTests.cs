using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.Feed.Client;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpFeedsTests
    {
        private readonly IAmazonSellingPartnerFeedClient _client;

        public AmazonSpFeedsTests()
        {
            _client = new DefaultSellingPartnerClientFactory().CreateFeedClient();
        }

        [RunnableInDebugOnly]
        public async Task Should_upload_inventory_loader_feed_end_to_end()
        {
            var marketplaceIds = new List<string>() { AmazonMarketplace.UK.MarketplaceId };
            var feedType = "POST_FLAT_FILE_INVLOADER_DATA";
            var contentType = "text/tab-separated-values; charset=utf-8"; // "text/xml; charset=UTF-8"
            
            var feedData = @"sku	merchant-shipping-group-name
5249	uk|courier|std|0";

            var createFeedDocumentResponse = await _client.CreateFeedDocumentAsync(new CreateFeedDocumentSpecification()
            {
                ContentType = contentType
            });

         
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Put, createFeedDocumentResponse.Url);
            request.Content = new StringContent(feedData, Encoding.UTF8, "text/tab-separated-values");

            var uploadFeedResponse = await httpClient.SendAsync(request);
            uploadFeedResponse.EnsureSuccessStatusCode();

            var createFeedResponse = await _client.CreateFeedAsync(new CreateFeedSpecification()
            {
                InputFeedDocumentId = createFeedDocumentResponse.FeedDocumentId,
                FeedType = feedType,
                MarketplaceIds = marketplaceIds,
            });

            FeedProcessingStatus status;
            string feedDocumentId;
            do
            {
                await Task.Delay(15000);
                var feedResponse = await _client.GetFeedAsync(createFeedResponse.FeedId);
                status = feedResponse.ProcessingStatus;
                feedDocumentId = feedResponse.ResultFeedDocumentId;
            } while (status != FeedProcessingStatus.DONE);

            var feedDocumentResponse = await _client.GetFeedDocumentAsync(feedDocumentId);

            var response = await httpClient.GetAsync(feedDocumentResponse.Url);
            var filePath = Path.Combine(Path.GetTempPath(), feedType + (feedDocumentResponse.CompressionAlgorithm == FeedDocumentCompressionAlgorithm.GZIP ? ".gz" : ".data"));

            await using (var fs = new FileStream(filePath, FileMode.Create))
                await response.Content.CopyToAsync(fs);

            var bytes = await File.ReadAllBytesAsync(filePath);
            bytes.Should().NotBeNull();
            bytes.Should().NotBeEmpty();

            if (feedDocumentResponse.CompressionAlgorithm == FeedDocumentCompressionAlgorithm.GZIP)
                await GzipUtil.DecompressAsync(filePath);
        }
    }
}