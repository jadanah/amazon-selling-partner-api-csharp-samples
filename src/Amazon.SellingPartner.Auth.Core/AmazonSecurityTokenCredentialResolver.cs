using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace Amazon.SellingPartner.Auth.Core
{
    public class AmazonSecurityTokenCredentialResolver : IAmazonSecurityTokenCredentialResolver
    {
        private readonly string _accessKey;
        private readonly string _host;

        private readonly RegionEndpoint _region;
        private readonly string _roleArn;
        private readonly int _roleDurationSeconds;
        private readonly Func<string> _roleSessionNameGenerator;
        private readonly string _secretKey;

        public AmazonSecurityTokenCredentialResolver(string host, string roleArn, string accessKey, string secretKey, RegionEndpoint region, int roleDurationSeconds = 3600,
            Func<string>? roleSessionNameGenerator = null)
        {
            _host = new Uri(host).Host;
            _roleArn = roleArn;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _region = region;
            _roleDurationSeconds = roleDurationSeconds;
            _roleSessionNameGenerator = roleSessionNameGenerator ?? (() => Guid.NewGuid().ToString());
        }

        public async Task<AmazonSecurityTokenCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default)
        {
            AssumeRoleResponse response = null;
            using (var stsClient = new AmazonSecurityTokenServiceClient(_accessKey, _secretKey, _region))
            {
                var req = new AssumeRoleRequest()
                {
                    RoleArn = _roleArn,
                    DurationSeconds = _roleDurationSeconds,
                    RoleSessionName = _roleSessionNameGenerator.Invoke()
                };

                response = await stsClient.AssumeRoleAsync(req, cancellationToken);
            }

            return new AmazonSecurityTokenCredentials
            {
                SessionToken = response.Credentials.SessionToken,
                SecretAccessKey = response.Credentials.SecretAccessKey,
                AccessKeyId = response.Credentials.AccessKeyId,
                Expiration = response.Credentials.Expiration,
                Region = _region.SystemName,
                Host = _host
            };
        }
    }
}