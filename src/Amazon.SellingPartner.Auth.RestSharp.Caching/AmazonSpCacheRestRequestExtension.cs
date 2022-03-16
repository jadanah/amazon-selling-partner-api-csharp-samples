using System;
using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;

namespace Amazon.SellingPartner.Auth.RestSharp.Caching
{
    public static class AmazonSpCacheRestRequestExtension
    {
        public static IRestRequest SignWithCachedAccessToken(this IRestRequest restRequest, IMemoryCache memoryCache, string clientId, string clientSecret, string refreshToken)
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = AmazonSpRestRequestUtil.GetLwaAuthorizationCredentials(clientId, clientSecret, refreshToken);
            return new MemoryCacheRestSharpLwaAuthorizationSigner(lwaAuthorizationCredentials, memoryCache).Sign(restRequest);
        }


        public static async Task<IRestRequest> SignWithCachedStsKeysAndSecurityTokenAsync(this IRestRequest restRequest, IMemoryCache memoryCache, string host, string roleArn, string accessKey,
            string secretKey, RegionEndpoint region)
        {
            var key = $"amazon:sts:{host}:{roleArn}:{accessKey}:{region.SystemName}:token";

            AssumeRoleResponse response = await memoryCache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600 - (60 * 5));
                return await AmazonSpRestRequestUtil.AssumeRoleAsync(roleArn, accessKey, secretKey, region);
            });

            return AmazonSpRestRequestUtil.AddSecurityTokenAndV4Sign(restRequest, host, region, response);
        }
    }
}