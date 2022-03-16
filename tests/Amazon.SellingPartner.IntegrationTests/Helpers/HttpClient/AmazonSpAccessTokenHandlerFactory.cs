using System;
using System.Net.Http;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.HttpClient;
using Amazon.SellingPartner.Auth.HttpClient.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient
{
    public class AmazonSpAccessTokenHandlerFactory
    {
        public AmazonSpAccessTokenHandler Create(SellingPartnerApiCredentials credentials, string endpoint, RegionEndpoint region)
        {
            return InternalCreateV2(credentials, endpoint, region);
        }

        private AmazonSpAccessTokenHandler InternalCreateV1(SellingPartnerApiCredentials credentials, string endpoint, RegionEndpoint region)
        {
            AmazonSpAccessTokenHandler pipeline = new AmazonSpAccessTokenHandler(credentials.ClientId, credentials.ClientSecret, credentials.RefreshToken)
            {
                InnerHandler = new AmazonSpSecurityTokenHandler(new AmazonSecurityTokenCredentialResolver(endpoint, credentials.RoleARN, credentials.AWSKey, credentials.AWSSecret,
                    region))
                {
                    InnerHandler = new HttpClientHandler()
                }
            };

            return pipeline;
        }

        private AmazonSpAccessTokenHandler InternalCreateV2(SellingPartnerApiCredentials credentials, string endpoint, RegionEndpoint region)
        {
            var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
            {
                ClientId = credentials.ClientId,
                ClientSecret = credentials.ClientSecret,
                Endpoint = new Uri("https://api.amazon.com/auth/o2/token"),
                RefreshToken = credentials.RefreshToken,
            };

            AmazonSpAccessTokenHandler pipeline = new AmazonSpAccessTokenHandler(new MemoryCacheHttpLwaClient(lwaAuthorizationCredentials, new MemoryCache(new MemoryCacheOptions())))
            {
                InnerHandler = new AmazonSpSecurityTokenHandler(new AmazonSecurityTokenCredentialResolver(endpoint, credentials.RoleARN, credentials.AWSKey, credentials.AWSSecret,
                    region))
                {
                    InnerHandler = new HttpClientHandler()
                }
            };

            return pipeline;
        }
    }
}