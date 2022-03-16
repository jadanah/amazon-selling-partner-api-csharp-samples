using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public class RestSharpLwaClient : ILwaClient
    {
        public const string AccessTokenKey = "access_token";
        public const string RefreshTokenKey = "refresh_token";
        public const string TokenTypeKey = "token_type";
        public const string ExpiresInKey = "expires_in";

        public const string JsonMediaType = "application/json; charset=utf-8";


        public RestSharpLwaClient(LwaAuthorizationCredentials lwaAuthorizationCredentials)
        {
            LWAAuthorizationCredentials = lwaAuthorizationCredentials;
            LWAAccessTokenRequestMetaBuilder = new LwaAccessTokenRequestMetaBuilder();
            var baseUrl = LWAAuthorizationCredentials.Endpoint.GetLeftPart(UriPartial.Authority);
            RestClient = new RestClient(baseUrl);
        }

        public IRestClient RestClient { get; set; }
        public LwaAccessTokenRequestMetaBuilder LWAAccessTokenRequestMetaBuilder { get; set; }
        public LwaAuthorizationCredentials LWAAuthorizationCredentials { get; private set; }

        public Task<LwaTokenResponse> GetTokenResponseAsync() => Task.FromResult(GetTokenResponse());

        public Task<string> GetAccessTokenAsync() => Task.FromResult(GetAccessToken());

        public virtual LwaTokenResponse GetTokenResponse()
        {
            LwaAccessTokenRequestMeta lwaAccessTokenRequestMeta = LWAAccessTokenRequestMetaBuilder.Build(LWAAuthorizationCredentials);
            var accessTokenRequest = new RestRequest(LWAAuthorizationCredentials.Endpoint.AbsolutePath, Method.POST);

            string jsonRequestBody = JsonConvert.SerializeObject(lwaAccessTokenRequestMeta);

            accessTokenRequest.AddParameter(JsonMediaType, jsonRequestBody, ParameterType.RequestBody);

            LwaTokenResponse accessToken = new LwaTokenResponse();
            try
            {
                var response = RestClient.Execute(accessTokenRequest);

                if (!IsSuccessful(response))
                {
                    throw new IOException("Unsuccessful LWA token exchange", response.ErrorException);
                }

                JObject responseJson = JObject.Parse(response.Content);

                accessToken.AccessToken = responseJson.GetValue(AccessTokenKey)?.ToString();
                accessToken.RefreshToken = responseJson.GetValue(RefreshTokenKey)?.ToString();
                accessToken.TokenType = responseJson.GetValue(TokenTypeKey)?.ToString();
                accessToken.ExpiresIn = responseJson.GetValue(ExpiresInKey)?.ToObject<int>();
            }
            catch (Exception e)
            {
                throw new SystemException("Error getting LWA Access Token", e);
            }

            return accessToken;
        }

        /// <summary>
        /// Retrieves access token from LWA
        /// </summary>
        /// <param name="lwaAccessTokenRequestMeta">LWA AccessTokenRequest metadata</param>
        /// <returns>LWA Access Token</returns>
        public virtual string GetAccessToken()
        {
            var token = GetTokenResponse();
            return token.AccessToken;
        }

        private bool IsSuccessful(IRestResponse response)
        {
            int statusCode = (int)response.StatusCode;
            return statusCode >= 200 && statusCode <= 299 && response.ResponseStatus == ResponseStatus.Completed;
        }
    }
}