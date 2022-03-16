namespace Amazon.SellingPartner.Auth.Core
{
    public class LwaAccessTokenRequestMetaBuilder
    {
        public const string SellerApiGrantType = "refresh_token";
        public const string SellerlessApiGrantType = "client_credentials";

        private const string Delimiter = " ";

        /// <summary>
        /// Builds an instance of LwaAccessTokenRequestMeta modeling appropriate LWA token
        /// request params based on configured LwaAuthorizationCredentials
        /// </summary>
        /// <param name="lwaAuthorizationCredentials">LWA Authorization Credentials</param>
        /// <returns></returns>
        public virtual LwaAccessTokenRequestMeta Build(LwaAuthorizationCredentials lwaAuthorizationCredentials)
        {
            LwaAccessTokenRequestMeta lwaAccessTokenRequestMeta = new LwaAccessTokenRequestMeta()
            {
                ClientId = lwaAuthorizationCredentials.ClientId,
                ClientSecret = lwaAuthorizationCredentials.ClientSecret,
                RefreshToken = lwaAuthorizationCredentials.RefreshToken
            };

            if (lwaAuthorizationCredentials.Scopes == null || lwaAuthorizationCredentials.Scopes.Count == 0)
            {
                lwaAccessTokenRequestMeta.GrantType = SellerApiGrantType;
            }
            else
            {
                lwaAccessTokenRequestMeta.Scope = string.Join(Delimiter, lwaAuthorizationCredentials.Scopes);
                lwaAccessTokenRequestMeta.GrantType = SellerlessApiGrantType;
            }

            return lwaAccessTokenRequestMeta;
        }
    }
}