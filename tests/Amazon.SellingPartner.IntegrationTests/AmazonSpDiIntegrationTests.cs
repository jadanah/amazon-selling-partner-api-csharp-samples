using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Caching;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.HttpClient;
using Amazon.SellingPartner.Auth.HttpClient.Caching;
using Amazon.SellingPartner.Core;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using Amazon.SellingPartner.Orders.Client;
using Amazon.SellingPartner.Sales.Client;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class AmazonSpDiIntegrationTests
    {
        public AmazonSpDiIntegrationTests()
        {
        }

        [Fact]
        public async Task Should_integrate_with_di_1()
        {
            // get credentials
            var amazonCredentials = new SellingPartnerApiCredentialsFactory().CreateFromUserSecrets();

            var awsKey = amazonCredentials.AWSKey;
            var awsSecret = amazonCredentials.AWSSecret;
            var roleArn = amazonCredentials.RoleARN;
            var clientId = amazonCredentials.ClientId;
            var clientSecret = amazonCredentials.ClientSecret;
            var refreshToken = amazonCredentials.RefreshToken;
            
            var spEndpoint = AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
            var lwaEndpoint = EndpointConstants.LwaToken;
            var region = RegionEndpoint.EUWest1;


            // add services
            var services = new ServiceCollection();

            // cache for token
            services.AddMemoryCache();

            // message handlers

            // services.AddTransient<AmazonSpAccessTokenHandler>(sp =>
            //     new AmazonSpAccessTokenHandler(clientId, clientSecret, refreshToken, lwaEndpoint));

            // or if you have registered a ILwaClient
            services.AddTransient<AmazonSpAccessTokenHandler>();
            services.AddTransient<AmazonSpSecurityTokenHandler>();

            services.AddScoped<IAmazonSecurityTokenCredentialResolver>(sp =>
                new MemoryCacheAmazonSecurityTokenCredentialResolver(sp.GetRequiredService<IMemoryCache>(), spEndpoint, roleArn,
                    awsKey, awsSecret, region, 3600, () => Guid.NewGuid().ToString()));

            // lwa client
            services.AddHttpClient("amzn-lwa", httpClient => { httpClient.BaseAddress = new Uri(lwaEndpoint); });
            // services.AddScoped<ILwaClient>(sp => new HttpLwaClient(lwaCredentials));
            services.AddScoped<ILwaClient>(sp =>
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var lwaCredentials = new LwaAuthorizationCredentials
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    RefreshToken = refreshToken,
                    Endpoint = new Uri(lwaEndpoint),
                    Scopes = null
                };
                return new MemoryCacheHttpLwaClient(clientFactory.CreateClient("amzn-lwa"), lwaCredentials, sp.GetRequiredService<IMemoryCache>());
            });

            // order api clients

            // services.AddHttpClient("amzn-sp-orders", httpClient => { httpClient.BaseAddress = new Uri(spEndpoint); })
            //     .AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
            //     .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();
            // services.AddScoped<IAmazonSellingPartnerOrdersClient, AmazonSellingPartnerOrdersClient>(sp =>
            // {
            //     var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
            //     return new AmazonSellingPartnerOrdersClient(clientFactory.CreateClient("amzn-sp-orders"))
            //     {
            //         JsonSerializerSettings =
            //         {
            //             // needed if some required fields are not going to be returned due to PII restrictions
            //             ContractResolver = new AmazonSellingPartnerSafeContractResolver()
            //         }
            //     };
            // });
            //

            services.AddHttpClient<IAmazonSellingPartnerOrdersClient, AmazonSellingPartnerOrdersClient>(client =>
                {
                    client.BaseAddress = new Uri(spEndpoint);
                    return new AmazonSellingPartnerOrdersClient(client)
                    {
                        JsonSerializerSettings =
                        {
                            // needed if some required fields are not going to be returned due to PII restrictions
                            ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                        }
                    };
                }).AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
                .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();

            // sales api clients

            // services.AddHttpClient("amzn-sp-sales", httpClient => { httpClient.BaseAddress = new Uri(spEndpoint); })
            //     .AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
            //     .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();
            //
            // services.AddScoped<IAmazonSellingPartnerSalesClient, AmazonSellingPartnerSalesClient>(sp =>
            // {
            //     var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
            //     return new AmazonSellingPartnerSalesClient(clientFactory.CreateClient("amzn-sp-sales"));
            // });
            //

            services.AddHttpClient<IAmazonSellingPartnerSalesClient, AmazonSellingPartnerSalesClient>(client =>
                {
                    client.BaseAddress = new Uri(spEndpoint);
                    return new AmazonSellingPartnerSalesClient(client);
                }).AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
                .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();

            // build service provider
            var serviceProvider = services.BuildServiceProvider();

            // resolve services and call api
            var salesClient = serviceProvider.GetRequiredService<IAmazonSellingPartnerSalesClient>();
            var ordersClient = serviceProvider.GetRequiredService<IAmazonSellingPartnerOrdersClient>();

            var startDate = new DateTime(2022, 03, 01, 00, 00, 00, DateTimeKind.Utc);
            var endDate = new DateTime(2022, 03, 07, 00, 00, 00, DateTimeKind.Utc);

            var salesResponse = await salesClient.GetOrderMetricsAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, AmazonDateUtil.ConvertToString(startDate, endDate), Granularity.Total);

            // access token from 1st call will be cached and used again for the 2nd request
            var ordersResponse = await ordersClient.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: startDate.ToAmazonDateTimeString(),
                createdBefore: endDate.ToAmazonDateTimeString());

            salesResponse.Should().NotBeNull();
            salesResponse.Payload.Should().NotBeNull();
            ordersResponse.Should().NotBeNull();
            ordersResponse.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_integrate_without_di_1()
        {
            // get credentials
            // get credentials
            var amazonCredentials = new SellingPartnerApiCredentialsFactory().CreateFromUserSecrets();

            var awsKey = amazonCredentials.AWSKey;
            var awsSecret = amazonCredentials.AWSSecret;
            var roleArn = amazonCredentials.RoleARN;
            var clientId = amazonCredentials.ClientId;
            var clientSecret = amazonCredentials.ClientSecret;
            var refreshToken = amazonCredentials.RefreshToken;
            
            var spEndpoint = AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
            var lwaEndpoint = EndpointConstants.LwaToken;
            var region = RegionEndpoint.EUWest1;

            IAmazonSecurityTokenCredentialResolver securityTokenResolver = new AmazonSecurityTokenCredentialResolver(spEndpoint, roleArn, awsKey, awsSecret, region);

            var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RefreshToken = refreshToken,
                Endpoint = new Uri(lwaEndpoint),
                Scopes = null
            };

            ILwaClient lwaClient = new HttpLwaClient(lwaAuthorizationCredentials);

            AmazonSpAccessTokenHandler pipeline = new AmazonSpAccessTokenHandler(lwaClient)
            {
                InnerHandler = new AmazonSpSecurityTokenHandler(securityTokenResolver)
                {
                    InnerHandler = new HttpClientHandler()
                }
            };

            var httpClient = new HttpClient(pipeline)
            {
                BaseAddress = new Uri(spEndpoint)
            };
            IAmazonSellingPartnerOrdersClient ordersClient = new AmazonSellingPartnerOrdersClient(httpClient)
            {
                JsonSerializerSettings =
                {
                    // needed if some required fields are not going to be returned due to PII restrictions
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };

            var startDate = new DateTime(2022, 03, 01, 00, 00, 00, DateTimeKind.Utc);
            var endDate = new DateTime(2022, 03, 07, 00, 00, 00, DateTimeKind.Utc);

            var ordersResponse = await ordersClient.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: startDate.ToAmazonDateTimeString(),
                createdBefore: endDate.ToAmazonDateTimeString());

            ordersResponse.Should().NotBeNull();
            ordersResponse.Payload.Should().NotBeNull();
        }
    }
}