using System;
using System.Collections.Generic;

namespace Amazon.SellingPartner.Auth.Core
{
    public class LwaAuthorizationCredentials
    {
        public LwaAuthorizationCredentials()
        {
            Scopes = new List<string>();
        }

        /// <summary>
        /// LWA Client Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// LWA Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// LWA Refresh Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// LWA Authorization Server Endpoint
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// LWA Authorization Scopes
        /// </summary>
        public List<string> Scopes { get; set; }
    }
}