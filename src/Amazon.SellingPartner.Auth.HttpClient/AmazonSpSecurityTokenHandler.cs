using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;

namespace Amazon.SellingPartner.Auth.HttpClient
{
    public class AmazonSpSecurityTokenHandler : DelegatingHandler
    {
        public const string SecurityTokenHeaderName = "x-amz-security-token";

        private readonly IAmazonSecurityTokenCredentialResolver _securityTokenCredentialResolver;

        public AmazonSpSecurityTokenHandler(IAmazonSecurityTokenCredentialResolver securityTokenCredentialResolver)
        {
            _securityTokenCredentialResolver = securityTokenCredentialResolver;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AmazonSecurityTokenCredentials response = await _securityTokenCredentialResolver.GetCredentialsAsync(cancellationToken);

            request.Headers.Add(SecurityTokenHeaderName, response.SessionToken);

            var awsAuthenticationCredentials = new AwsAuthenticationCredentials
            {
                AccessKeyId = response.AccessKeyId,
                SecretKey = response.SecretAccessKey,
                Region = response.Region
            };

            var signer = new HttpRequestMessageAWSSigV4Signer(awsAuthenticationCredentials);
            request = signer.Sign(request, response.Host);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}