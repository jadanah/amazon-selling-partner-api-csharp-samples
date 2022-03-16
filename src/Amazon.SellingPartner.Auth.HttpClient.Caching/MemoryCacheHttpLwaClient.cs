using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.SellingPartner.Auth.HttpClient.Caching
{
    public class MemoryCacheHttpLwaClient : HttpLwaClient
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheHttpLwaClient(System.Net.Http.HttpClient httpClient, LwaAuthorizationCredentials lwaAuthorizationCredentials, IMemoryCache memoryCache) : base(httpClient,
            lwaAuthorizationCredentials)
        {
            _memoryCache = memoryCache;
        }

        public MemoryCacheHttpLwaClient(LwaAuthorizationCredentials lwaAuthorizationCredentials, IMemoryCache memoryCache) : base(lwaAuthorizationCredentials)
        {
            _memoryCache = memoryCache;
        }

        public override async Task<LwaTokenResponse> GetTokenResponseAsync()
        {
            var key = string.Format("amzn:lwa:{0}:{1}:{2}:token",
                LWAAuthorizationCredentials.ClientId,
                LWAAuthorizationCredentials.Endpoint.Host,
                string.Join("-", LWAAuthorizationCredentials?.Scopes ?? new List<string>()));

            return await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                // cache for 55 mins (token is valid for 60 mins)
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600 - (60 * 5));
                return await base.GetTokenResponseAsync();
            });
        }
    }
}