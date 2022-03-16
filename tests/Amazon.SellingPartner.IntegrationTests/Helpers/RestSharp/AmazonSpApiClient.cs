using System;
using Amazon.SellingPartner.Auth.RestSharp;
using Amazon.SellingPartner.RestSharp.Orders.Client;
using RestSharp;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.RestSharp
{
    public class AmazonSpApiClient : ApiClient
    {
        private readonly string _awsKey;
        private readonly string _awsSecret;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _host;
        private readonly string _refreshToken;
        private readonly RegionEndpoint _region;
        private readonly string _roleArn;

        public AmazonSpApiClient(string clientId, string clientSecret, string refreshToken, string host, string roleArn, string awsKey, string awsSecret, RegionEndpoint region) : base(
            "https://sellingpartnerapi-eu.amazon.com")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _host = new Uri(host).Host;
            _roleArn = roleArn;
            _awsKey = awsKey;
            _awsSecret = awsSecret;
            _region = region;
        }

        protected override void InterceptRequest(IRestRequest request)
        {
            request.SignWithAccessToken(_clientId, _clientSecret, _refreshToken);

            request.SignWithStsKeysAndSecurityTokenAsync(_host, _roleArn, _awsKey, _awsSecret, _region)
                .GetAwaiter()
                .GetResult();
        }
    }
}