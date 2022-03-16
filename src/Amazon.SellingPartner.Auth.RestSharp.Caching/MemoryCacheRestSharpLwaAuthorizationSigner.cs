using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.SellingPartner.Auth.RestSharp.Caching
{
    public class MemoryCacheRestSharpLwaAuthorizationSigner : RestSharpLwaAuthorizationSigner
    {
        public MemoryCacheRestSharpLwaAuthorizationSigner(LwaAuthorizationCredentials lwaAuthorizationCredentials, IMemoryCache memoryCache) : base(lwaAuthorizationCredentials)
        {
            RestSharpLwaClient = new MemoryCacheRestSharpLwaClient(lwaAuthorizationCredentials, memoryCache);
        }
    }
}