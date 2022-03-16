using Amazon.SellingPartner.Auth.Core;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public class RestSharpLwaAuthorizationSigner
    {
        public const string AccessTokenHeaderName = "x-amz-access-token";

        /// <summary>
        /// Constructor for RestSharpLwaAuthorizationSigner
        /// </summary>
        /// <param name="lwaAuthorizationCredentials">LWA Authorization Credentials for token exchange</param>
        public RestSharpLwaAuthorizationSigner(LwaAuthorizationCredentials lwaAuthorizationCredentials)
        {
            RestSharpLwaClient = new RestSharpLwaClient(lwaAuthorizationCredentials);
        }

        public RestSharpLwaClient RestSharpLwaClient { get; set; }

        /// <summary>
        /// Signs a request with LWA Access Token
        /// </summary>
        /// <param name="restRequest">Request to sign</param>
        /// <returns>restRequest with LWA signature</returns>
        public IRestRequest Sign(IRestRequest restRequest)
        {
            string accessToken = RestSharpLwaClient.GetAccessToken();

            restRequest.AddHeader(AccessTokenHeaderName, accessToken);

            return restRequest;
        }
    }
}