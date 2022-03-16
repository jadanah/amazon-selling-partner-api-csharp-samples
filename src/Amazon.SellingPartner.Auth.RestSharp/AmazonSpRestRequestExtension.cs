using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
using Amazon.SellingPartner.Auth.Core;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public static class AmazonSpRestRequestExtension
    {
        public static IRestRequest SignWithAccessToken(this IRestRequest restRequest, string clientId, string clientSecret, string refreshToken)
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = AmazonSpRestRequestUtil.GetLwaAuthorizationCredentials(clientId, clientSecret, refreshToken);
            return new RestSharpLwaAuthorizationSigner(lwaAuthorizationCredentials).Sign(restRequest);
        }


        public static async Task<IRestRequest> SignWithStsKeysAndSecurityTokenAsync(this IRestRequest restRequest, string host, string roleArn, string accessKey, string secretKey,
            RegionEndpoint region)
        {
            AssumeRoleResponse response = await AmazonSpRestRequestUtil.AssumeRoleAsync(roleArn, accessKey, secretKey, region);
            return AmazonSpRestRequestUtil.AddSecurityTokenAndV4Sign(restRequest, host, region, response);
        }
    }
}