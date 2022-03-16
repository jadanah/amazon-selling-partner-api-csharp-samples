using System;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.RestSharp;
using Moq;
using RestSharp;
using Xunit;

namespace Amazon.SellingPartner.Auth.Tests
{
    public class LwaAuthorizationSignerTest
    {
        private static readonly LwaAuthorizationCredentials LwaAuthorizationCredentials = new LwaAuthorizationCredentials()
        {
            ClientId = "cid",
            ClientSecret = "csecret",
            Endpoint = new Uri("https://www.amazon.com")
        };

        private RestSharpLwaAuthorizationSigner lwaAuthorizationSignerUnderTest;

        public LwaAuthorizationSignerTest()
        {
            lwaAuthorizationSignerUnderTest = new RestSharpLwaAuthorizationSigner(LwaAuthorizationCredentials);
        }

        [Fact]
        public void ConstructorInitializesLWAClientWithCredentials()
        {
            Assert.Equal(LwaAuthorizationCredentials, lwaAuthorizationSignerUnderTest.RestSharpLwaClient.LWAAuthorizationCredentials);
        }

        [Fact]
        public void RequestIsSignedFromLWAClientProvidedToken()
        {
            string expectedAccessToken = "foo";

            var mockLWAClient = new Mock<RestSharpLwaClient>(LwaAuthorizationCredentials);
            mockLWAClient.Setup(lwaClient => lwaClient.GetAccessToken()).Returns(expectedAccessToken);
            lwaAuthorizationSignerUnderTest.RestSharpLwaClient = mockLWAClient.Object;

            IRestRequest restRequest = new RestRequest();
            restRequest = lwaAuthorizationSignerUnderTest.Sign(restRequest);

            Parameter actualAccessTokenHeader = restRequest.Parameters.Find(parameter =>
                ParameterType.HttpHeader.Equals(parameter.Type) && parameter.Name == RestSharpLwaAuthorizationSigner.AccessTokenHeaderName);

            Assert.Equal(expectedAccessToken, actualAccessTokenHeader.Value);
        }
    }
}