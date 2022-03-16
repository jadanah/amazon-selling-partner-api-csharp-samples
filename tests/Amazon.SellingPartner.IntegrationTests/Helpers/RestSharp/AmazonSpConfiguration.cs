using Amazon.SellingPartner.RestSharp.Orders.Client;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.RestSharp
{
    public partial class AmazonSpConfiguration : GlobalConfiguration
    {
        private static string _clientId;
        private static string _clientSecret;
        private static string _refreshToken;
        private static string _host;
        private static string _roleArn;
        private static string _awsKey;
        private static string _awsSecret;
        private static RegionEndpoint _region;
        public override string BasePath { get; set; } = "https://sellingpartnerapi-eu.amazon.com";
        public override AmazonSpApiClient ApiClient { get; } = GetClient();

        private static AmazonSpApiClient GetClient()
        {
            return new AmazonSpApiClient(_clientId, _clientSecret, _refreshToken, _host, _roleArn, _awsKey, _awsSecret, _region);
        }

        public static void SetClientParams(string clientId, string clientSecret, string refreshToken, string host, string roleArn, string awsKey, string awsSecret, RegionEndpoint region)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _host = host;
            _roleArn = roleArn;
            _awsKey = awsKey;
            _awsSecret = awsSecret;
            _region = region;
        }
    }
}