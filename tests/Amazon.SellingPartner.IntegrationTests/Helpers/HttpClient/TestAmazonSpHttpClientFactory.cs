using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Core;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient
{
    public class TestAmazonSpHttpClientFactory
    {
        public System.Net.Http.HttpClient Create(SellingPartnerApiCredentials? credentials = null, string endpoint = null, RegionEndpoint region = null)
        {
            credentials ??= new SellingPartnerApiCredentialsFactory().CreateFromUserSecrets();
            endpoint ??= AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
            region ??= RegionEndpoint.EUWest1;

            return new AmazonSpHttpClientFactory().Create(credentials, endpoint, region);
        }
    }
}