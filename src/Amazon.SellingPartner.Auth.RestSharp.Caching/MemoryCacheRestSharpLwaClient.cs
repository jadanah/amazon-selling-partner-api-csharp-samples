using System;
using System.Collections.Generic;
using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.SellingPartner.Auth.RestSharp.Caching
{
    public class MemoryCacheRestSharpLwaClient : RestSharpLwaClient
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheRestSharpLwaClient(LwaAuthorizationCredentials lwaAuthorizationCredentials, IMemoryCache memoryCache) : base(lwaAuthorizationCredentials)
        {
            _memoryCache = memoryCache;
        }

        public override LwaTokenResponse GetTokenResponse()
        {
            var key = string.Format("amazon:lwa:{0}:{1}:{2}:token",
                LWAAuthorizationCredentials.ClientId,
                LWAAuthorizationCredentials.Endpoint.Host,
                string.Join("-", LWAAuthorizationCredentials?.Scopes ?? new List<string>()));

            return _memoryCache.GetOrCreate(key, entry =>
            {
                // cache for 60 mins - 5 mins safety margin
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600 - (60 * 5));
                return base.GetTokenResponse();
            });
        }
    }
}