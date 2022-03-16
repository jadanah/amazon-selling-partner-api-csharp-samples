using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.SellingPartner.Auth.Caching
{
    public class MemoryCacheAmazonSecurityTokenCredentialResolver : AmazonSecurityTokenCredentialResolver, IAmazonSecurityTokenCredentialResolver
    {
        private readonly string _accessKey;
        private readonly IMemoryCache _memoryCache;
        private readonly string _region;

        private readonly string _roleArn;

        public MemoryCacheAmazonSecurityTokenCredentialResolver(IMemoryCache memoryCache, string host, string roleArn, string accessKey, string secretKey, RegionEndpoint region,
            int roleDurationSeconds = 3600, Func<string>? roleSessionNameGenerator = null) : base(host, roleArn, accessKey, secretKey, region, roleDurationSeconds, roleSessionNameGenerator)
        {
            _memoryCache = memoryCache;
            _roleArn = roleArn;
            _accessKey = accessKey;
            _region = region.SystemName;
        }

        public new async Task<AmazonSecurityTokenCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default)
        {
            var key = string.Format("amzn:security-session:{0}:{1}:{2}",
                _region,
                _roleArn,
                _accessKey);

            return await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                AmazonSecurityTokenCredentials response = await base.GetCredentialsAsync(cancellationToken);
                entry.AbsoluteExpiration = new DateTimeOffset(response.Expiration.GetValueOrDefault()) - TimeSpan.FromMinutes(5);
                return response;
            });
        }
    }
}