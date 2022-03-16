using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.RestSharp;
using Amazon.SellingPartner.Auth.RestSharp.Caching;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.IntegrationTests.Helpers.RestSharp;
using Amazon.SellingPartner.Orders.Client;
using Amazon.SellingPartner.RestSharp.Orders.Api;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpClientIntegrationTests
    {
        private readonly SellingPartnerApiCredentials _credentials;
        private readonly string _endpoint;
        private readonly IMemoryCache _memoryCache;
        private readonly RegionEndpoint _region;
        private readonly ITestOutputHelper _testOutputHelper;

        public AmazonSpClientIntegrationTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            // https://github.com/amzn/selling-partner-api-docs/blob/main/guides/en-US/developer-guide/SellingPartnerApiDeveloperGuide.md#selling-partner-api-endpoints
            // ensure trailing slash
            _endpoint = AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
            _region = RegionEndpoint.EUWest1;

            _credentials = new SellingPartnerApiCredentialsFactory().CreateFromUserSecrets();

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }


        [Fact(Skip = "Issues with the ordersV0.json model marking properties as required but other than that it works")]
        public async Task Should_get_amazon_order_by_id_using_nswag_client()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new AmazonSpHttpClientFactory().Create(_credentials, _endpoint, _region);
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                // removed BaseUrl from client, needs to be set in HttpClient
                // BaseUrl = _endpoint,
                ReadResponseAsString = true // for debugging only
            };

            GetOrderResponse? response = await client.GetOrderAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
            response.Payload?.AmazonOrderId.Should().Be(orderId);
        }

        [Fact]
        public async Task Should_get_amazon_order_by_id_using_nswag_client_in_which_we_have_derived_with_our_own_contract_resolver()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new AmazonSpHttpClientFactory().Create(_credentials, _endpoint, _region);
            var client = new SafeAmazonSellingPartnerOrdersClient(httpClient)
            {
                // removed BaseUrl from client, needs to be set in HttpClient
                // BaseUrl = _endpoint,
                ReadResponseAsString = true // for debugging only
            };

            GetOrderResponse? response = await client.GetOrderAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
            response.Payload?.AmazonOrderId.Should().Be(orderId);
        }

        [Fact]
        public async Task Should_get_amazon_order_by_id_using_nswag_client_but_define_our_own_contract_resolver()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new AmazonSpHttpClientFactory().Create(_credentials, _endpoint, _region);
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                // removed BaseUrl from client, needs to be set in HttpClient
                // BaseUrl = _endpoint,
                ReadResponseAsString = true, // for debugging only
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            GetOrderResponse? response = await client.GetOrderAsync(orderId);

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
            response.Payload?.AmazonOrderId.Should().Be(orderId);
        }

        [Fact]
        public async Task Should_get_raw_amazon_order_by_date_using_nswag_client()
        {
            var httpClient = new AmazonSpHttpClientFactory().Create(_credentials, _endpoint, _region);
            var client = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                // removed BaseUrl from client, needs to be set in HttpClient
                // BaseUrl = _endpoint,
                ReadResponseAsString = true, // for debugging only
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            GetOrdersResponse? response = await client.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: "2022-03-07T00:00:00Z", createdBefore: "2022-03-09T00:00:00Z");

            response.Should().NotBeNull();
            response.Payload.Should().NotBeNull();
            response.Payload?.Orders.Should().HaveCountGreaterThan(1);
        }

        [Fact(Skip = "Forced BaseUrl to be set in HttpClient so test is no longer needed")]
        public async Task Should_throw_exception_if_you_have_tried_to_define_endpoint_within_the_http_client()
        {
            var orderId = "026-3243799-6137111";

            var httpClient = new AmazonSpHttpClientFactory().Create(_credentials, _endpoint, _region);
            var client = new SafeAmazonSellingPartnerOrdersClient(httpClient)
            {
                // removed BaseUrl from client, needs to be set in HttpClient
                // BaseUrl = _endpoint,
                ReadResponseAsString = true, // for debugging only
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            Func<Task> act = async () => { await client.GetOrderAsync(orderId); };
            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Should_get_raw_amazon_order_by_id_using_restsharp_client()
        {
            var orderId = "026-3243799-6137111";
            var resource = $"/orders/v0/orders/{orderId}";
            IRestRequest restRequest = new RestRequest(resource, Method.GET);

            var client = new RestClient(_endpoint);

            restRequest.SignWithAccessToken(_credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken);
            await restRequest.SignWithStsKeysAndSecurityTokenAsync(client.BaseUrl!.Host, _credentials.RoleARN, _credentials.AWSKey, _credentials.AWSSecret, _region);

            IRestResponse? response = await client.ExecuteAsync(restRequest);

            response.Content.Should().NotBeNull();
            response.Content.Should().NotBeEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Should_get_raw_amazon_order_by_date_using_restsharp_client()
        {
            var resource = $"/orders/v0/orders?MarketplaceIds=A1F83G8C2ARO7P&CreatedAfter=2022-03-07T00:00:00Z&CreatedBefore=2022-03-09T00:00:00Z";
            IRestRequest restRequest = new RestRequest(resource, Method.GET);

            var client = new RestClient(_endpoint);

            restRequest.SignWithAccessToken(_credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken);
            await restRequest.SignWithStsKeysAndSecurityTokenAsync(client.BaseUrl!.Host, _credentials.RoleARN, _credentials.AWSKey, _credentials.AWSSecret, _region);

            IRestResponse? response = await client.ExecuteAsync(restRequest);

            response.Content.Should().NotBeNull();
            response.Content.Should().NotBeEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_get_raw_amazon_order_by_id_using_restsharp_client_with_caching_tokens()
        {
            var orderId = "026-3243799-6137111";
            var resource = $"/orders/v0/orders/{orderId}";
            IRestRequest restRequest = new RestRequest(resource, Method.GET);

            var client = new RestClient(_endpoint);

            restRequest.SignWithCachedAccessToken(_memoryCache, _credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken);
            await restRequest.SignWithCachedStsKeysAndSecurityTokenAsync(_memoryCache, client.BaseUrl!.Host, _credentials.RoleARN, _credentials.AWSKey, _credentials.AWSSecret, _region);

            IRestResponse? response = await client.ExecuteAsync(restRequest);

            response.Content.Should().NotBeNull();
            response.Content.Should().NotBeEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // 2nd request which should use cache

            IRestRequest restRequest2 = new RestRequest(resource, Method.GET);

            var client2 = new RestClient(_endpoint);

            restRequest2.SignWithCachedAccessToken(_memoryCache, _credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken);
            await restRequest2.SignWithCachedStsKeysAndSecurityTokenAsync(_memoryCache, client2.BaseUrl!.Host, _credentials.RoleARN, _credentials.AWSKey, _credentials.AWSSecret, _region);

            IRestResponse? response2 = await client2.ExecuteAsync(restRequest2);

            response2.Content.Should().NotBeNull();
            response2.Content.Should().NotBeEmpty();
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(Skip =
            "Issues with the ordersV0.json model marking properties as required but other than that it works. Even if we use a customer resolver the model created by swagger codegen contains null checks within ctor so will throw")]
        public async Task Should_get_amazon_order_by_id_using_swagger_codegen_restsharp_client()
        {
            var orderId = "026-3243799-6137111";

            AmazonSpConfiguration.SetClientParams(_credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken, _endpoint, _credentials.RoleARN, _credentials.AWSKey,
                _credentials.AWSSecret, _region);
            var client = new OrdersV0Api(new AmazonSpConfiguration()
            {
                BasePath = _endpoint,
            });

            var response = await client.GetOrderAsyncWithHttpInfo(orderId);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
        }

        [Fact(Skip =
            "Issues with the ordersV0.json model marking properties as required but other than that it works. Even if we use a customer resolver the model created by swagger codegen contains null checks within ctor so will throw")]
        public async Task Should_get_amazon_order_by_id_using_swagger_codegen_restsharp_client_using_our_safe_contract_resolver()
        {
            var orderId = "026-3243799-6137111";

            AmazonSpConfiguration.SetClientParams(_credentials.ClientId, _credentials.ClientSecret, _credentials.RefreshToken, _endpoint, _credentials.RoleARN, _credentials.AWSKey,
                _credentials.AWSSecret, _region);
            var client = new OrdersV0Api(new AmazonSpConfiguration()
            {
                BasePath = _endpoint,
            });
            client.Configuration.ApiClient.RestClient.AddHandler("application/json", new SafeRestSharpJsonNetSerializer());

            var response = await client.GetOrderAsyncWithHttpInfo(orderId);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
        }
    }
}