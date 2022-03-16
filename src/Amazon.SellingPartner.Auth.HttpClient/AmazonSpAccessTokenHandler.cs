using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;

namespace Amazon.SellingPartner.Auth.HttpClient
{
    public class AmazonSpAccessTokenHandler : DelegatingHandler
    {
        public const string AccessTokenHeaderName = "x-amz-access-token";

        private readonly ILwaClient _lwaClient;

        public AmazonSpAccessTokenHandler(string clientId, string clientSecret, string refreshToken, string endpoint = "https://api.amazon.com/auth/o2/token") : this(clientId, clientSecret,
            refreshToken, new Uri(endpoint))
        {
        }

        public AmazonSpAccessTokenHandler(string clientId, string clientSecret, string refreshToken, Uri endpoint)
        {
            var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Endpoint = endpoint,
                RefreshToken = refreshToken,
            };

            _lwaClient = new HttpLwaClient(lwaAuthorizationCredentials);
        }

        public AmazonSpAccessTokenHandler(ILwaClient lwaClient)
        {
            _lwaClient = lwaClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken = await _lwaClient.GetAccessTokenAsync();

            request.Headers.Add(AccessTokenHeaderName, accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}