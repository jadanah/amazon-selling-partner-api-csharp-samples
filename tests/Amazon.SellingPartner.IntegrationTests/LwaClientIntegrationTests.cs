using System;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.HttpClient;
using Amazon.SellingPartner.Auth.RestSharp;
using Amazon.SellingPartner.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Amazon.SellingPartner.IntegrationTests
{
    public class LwaClientIntegrationTests
    {
        [Fact]
        public async Task Should_get_access_token_using_restsharp()
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = GetLwaAuthorizationCredentials();
            ILwaClient client = new RestSharpLwaClient(lwaAuthorizationCredentials);

            var response = await client.GetAccessTokenAsync();

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_get_access_token_using_httpclient()
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = GetLwaAuthorizationCredentials();
            ILwaClient client = new HttpLwaClient(lwaAuthorizationCredentials);

            var response = await client.GetAccessTokenAsync();

            response.Should().NotBeNull();
        }

        private static LwaAuthorizationCredentials GetLwaAuthorizationCredentials()
        {
            var credentials = new SellingPartnerApiCredentialsFactory().CreateFromUserSecrets();
            var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
            {
                ClientId = credentials.ClientId,
                ClientSecret = credentials.ClientSecret,
                Endpoint = new Uri(EndpointConstants.LwaToken),
                RefreshToken = credentials.RefreshToken,
            };
            return lwaAuthorizationCredentials;
        }
    }
}