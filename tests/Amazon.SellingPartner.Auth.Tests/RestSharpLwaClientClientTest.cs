using System;
using System.IO;
using System.Linq;
using System.Net;
using Amazon.SellingPartner.Auth.Core;
using Amazon.SellingPartner.Auth.RestSharp;
using Moq;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

namespace Amazon.SellingPartner.Auth.Tests
{
    public class RestSharpLwaClientClientTest
    {
        private const string TestClientSecret = "cSecret";
        private const string TestClientId = "cId";
        private const string TestRefreshGrantType = "rToken";

        private static readonly Uri TestEndpoint = new Uri("https://www.amazon.com/lwa");

        private static readonly LwaAuthorizationCredentials LWAAuthorizationCredentials = new LwaAuthorizationCredentials
        {
            ClientId = TestClientId,
            ClientSecret = TestClientSecret,
            RefreshToken = TestRefreshGrantType,
            Endpoint = TestEndpoint
        };

        private static readonly IRestResponse Response = new RestResponse
        {
            StatusCode = HttpStatusCode.OK,
            ResponseStatus = ResponseStatus.Completed,
            Content = @"{access_token:'Azta|foo'}"
        };

        private Mock<LwaAccessTokenRequestMetaBuilder> mockLWAAccessTokenRequestMetaBuilder;
        private Mock<RestClient> mockRestClient;

        public RestSharpLwaClientClientTest()
        {
            mockRestClient = new Mock<RestClient>();
            mockLWAAccessTokenRequestMetaBuilder = new Mock<LwaAccessTokenRequestMetaBuilder>();
        }

        [Fact]
        public void InitializeLWAAuthorizationCredentials()
        {
            RestSharpLwaClient lwaClientUnderTest = new RestSharpLwaClient(LWAAuthorizationCredentials);
            Assert.Equal(LWAAuthorizationCredentials, lwaClientUnderTest.LWAAuthorizationCredentials);
        }

        [Fact]
        public void MakeRequestFromMeta()
        {
            IRestRequest request = new RestRequest();
            LwaAccessTokenRequestMeta expectedLWAAccessTokenRequestMeta = new LwaAccessTokenRequestMeta()
            {
                ClientSecret = "expectedSecret",
                ClientId = "expectedClientId",
                RefreshToken = "expectedRefreshToken",
                GrantType = "expectedGrantType"
            };

            mockRestClient.Setup(client => client.Execute(It.IsAny<IRestRequest>()))
                .Callback((IRestRequest req) => { request = req; })
                .Returns(Response);

            mockLWAAccessTokenRequestMetaBuilder.Setup(builder => builder.Build(LWAAuthorizationCredentials))
                .Returns(expectedLWAAccessTokenRequestMeta);

            RestSharpLwaClient lwaClientUnderTest = new RestSharpLwaClient(LWAAuthorizationCredentials);
            lwaClientUnderTest.RestClient = mockRestClient.Object;
            lwaClientUnderTest.LWAAccessTokenRequestMetaBuilder = mockLWAAccessTokenRequestMetaBuilder.Object;
            lwaClientUnderTest.GetAccessToken();

            Parameter requestBody = request.Parameters
                .FirstOrDefault(parameter => parameter.Type.Equals(ParameterType.RequestBody));

            JObject jsonRequestBody = JObject.Parse(requestBody.Value.ToString());

            Assert.Equal(Method.POST, request.Method);
            Assert.Equal(TestEndpoint.AbsolutePath, request.Resource);
            Assert.Equal(expectedLWAAccessTokenRequestMeta.RefreshToken, jsonRequestBody.GetValue("refresh_token"));
            Assert.Equal(expectedLWAAccessTokenRequestMeta.GrantType, jsonRequestBody.GetValue("grant_type"));
            Assert.Equal(expectedLWAAccessTokenRequestMeta.ClientId, jsonRequestBody.GetValue("client_id"));
            Assert.Equal(expectedLWAAccessTokenRequestMeta.ClientSecret, jsonRequestBody.GetValue("client_secret"));
        }

        [Fact]
        public void ReturnAccessTokenFromResponse()
        {
            IRestRequest request = new RestRequest();

            mockRestClient.Setup(client => client.Execute(It.IsAny<IRestRequest>()))
                .Callback((IRestRequest req) => { request = req; })
                .Returns(Response);

            RestSharpLwaClient lwaClientUnderTest = new RestSharpLwaClient(LWAAuthorizationCredentials);
            lwaClientUnderTest.RestClient = mockRestClient.Object;

            string accessToken = lwaClientUnderTest.GetAccessToken();

            Assert.Equal("Azta|foo", accessToken);
        }

        [Fact]
        public void UnsuccessfulPostThrowsException()
        {
            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseStatus = ResponseStatus.Completed,
                Content = string.Empty
            };

            IRestRequest request = new RestRequest();

            mockRestClient.Setup(client => client.Execute(It.IsAny<IRestRequest>()))
                .Callback((IRestRequest req) => { request = req; })
                .Returns(response);

            RestSharpLwaClient lwaClientUnderTest = new RestSharpLwaClient(LWAAuthorizationCredentials);
            lwaClientUnderTest.RestClient = mockRestClient.Object;

            SystemException systemException = Assert.Throws<SystemException>(() => lwaClientUnderTest.GetAccessToken());
            Assert.IsType<IOException>(systemException.GetBaseException());
        }

        [Fact]
        public void MissingAccessTokenInResponseThrowsException()
        {
            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed,
                Content = string.Empty
            };

            IRestRequest request = new RestRequest();

            mockRestClient.Setup(client => client.Execute(It.IsAny<RestRequest>()))
                .Callback((IRestRequest req) => { request = (RestRequest)req; })
                .Returns(response);

            RestSharpLwaClient lwaClientUnderTest = new RestSharpLwaClient(LWAAuthorizationCredentials);
            lwaClientUnderTest.RestClient = mockRestClient.Object;

            Assert.Throws<SystemException>(() => lwaClientUnderTest.GetAccessToken());
        }
    }
}