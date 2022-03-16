using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Newtonsoft.Json;

namespace Amazon.SellingPartner.Auth.HttpClient
{
    public class HttpLwaClient : ILwaClient
    {
        public const string AccessTokenKey = "access_token";
        public const string RefreshTokenKey = "refresh_token";
        public const string TokenTypeKey = "token_type";
        public const string ExpiresInKey = "expires_in";

        public const string JsonMediaType = "application/json; charset=utf-8";

        public HttpLwaClient(System.Net.Http.HttpClient httpClient, LwaAuthorizationCredentials lwaAuthorizationCredentials)
        {
            LWAAuthorizationCredentials = lwaAuthorizationCredentials;
            LWAAccessTokenRequestMetaBuilder = new LwaAccessTokenRequestMetaBuilder();
            HttpClient = httpClient;
        }

        public HttpLwaClient(LwaAuthorizationCredentials lwaAuthorizationCredentials) : this(
            new System.Net.Http.HttpClient() { BaseAddress = new Uri(lwaAuthorizationCredentials.Endpoint.GetLeftPart(UriPartial.Authority)) }, lwaAuthorizationCredentials)
        {
        }

        public System.Net.Http.HttpClient HttpClient { get; set; }
        public LwaAccessTokenRequestMetaBuilder LWAAccessTokenRequestMetaBuilder { get; set; }
        public LwaAuthorizationCredentials LWAAuthorizationCredentials { get; private set; }

        public virtual async Task<LwaTokenResponse> GetTokenResponseAsync()
        {
            LwaAccessTokenRequestMeta lwaAccessTokenRequestMeta = LWAAccessTokenRequestMetaBuilder.Build(LWAAuthorizationCredentials);

            var jsonRequestBody = JsonConvert.SerializeObject(lwaAccessTokenRequestMeta);

            var request = new HttpRequestMessage(HttpMethod.Post, LWAAuthorizationCredentials.Endpoint.AbsolutePath);
            request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            LwaTokenResponse accessToken;
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new IOException("Unsuccessful LWA token exchange");
                }

                var content = await response.Content.ReadAsStringAsync();

                accessToken = JsonConvert.DeserializeObject<LwaTokenResponse>(content);
            }
            catch (Exception e)
            {
                throw new SystemException("Error getting LWA Access Token", e);
            }

            return accessToken;
        }

        public virtual async Task<string> GetAccessTokenAsync()
        {
            LwaTokenResponse token = await GetTokenResponseAsync();
            return token.AccessToken;
        }
    }
}