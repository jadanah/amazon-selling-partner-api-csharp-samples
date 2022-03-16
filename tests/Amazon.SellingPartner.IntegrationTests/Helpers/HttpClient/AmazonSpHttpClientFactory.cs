using System;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.HttpClient;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient
{
    public class AmazonSpHttpClientFactory
    {
        public System.Net.Http.HttpClient Create(SellingPartnerApiCredentials credentials, string endpoint, RegionEndpoint region)
        {
            AmazonSpAccessTokenHandler pipeline = new AmazonSpAccessTokenHandlerFactory().Create(credentials, endpoint, region);
            var httpClient = new System.Net.Http.HttpClient(pipeline)
            {
                BaseAddress = new Uri(endpoint)
            };
            return httpClient;
        }
    }
}