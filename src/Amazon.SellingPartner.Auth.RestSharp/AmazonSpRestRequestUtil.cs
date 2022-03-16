using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Amazon.SellingPartner.Auth.Core;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public static class AmazonSpRestRequestUtil
    {
        public static LwaAuthorizationCredentials GetLwaAuthorizationCredentials(string clientId, string clientSecret, string refreshToken, string endpoint = "https://api.amazon.com/auth/o2/token")
        {
            var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Endpoint = new Uri(endpoint),
                RefreshToken = refreshToken,
            };
            return lwaAuthorizationCredentials;
        }

        public static IRestRequest AddSecurityTokenAndV4Sign(IRestRequest restRequest, string host, RegionEndpoint region, AssumeRoleResponse response)
        {
            restRequest.AddHeader("x-amz-security-token", response.Credentials.SessionToken);

            var awsAuthenticationCredentials = new AwsAuthenticationCredentials
            {
                AccessKeyId = response.Credentials.AccessKeyId,
                SecretKey = response.Credentials.SecretAccessKey,
                Region = region.SystemName
            };
            var signer = new RestSharpAwsSigV4Signer(awsAuthenticationCredentials);

            return signer.Sign(restRequest, host);
        }

        public static async Task<AssumeRoleResponse> AssumeRoleAsync(string roleArn, string accessKey, string secretKey, RegionEndpoint region, int durationSeconds = 3600)
        {
            AssumeRoleResponse response = null;
            using (var stsClient = new AmazonSecurityTokenServiceClient(accessKey, secretKey, region))
            {
                var req = new AssumeRoleRequest()
                {
                    RoleArn = roleArn,
                    DurationSeconds = durationSeconds,
                    RoleSessionName = Guid.NewGuid().ToString()
                };

                response = await stsClient.AssumeRoleAsync(req, new CancellationToken());
            }

            return response;
        }
    }
}