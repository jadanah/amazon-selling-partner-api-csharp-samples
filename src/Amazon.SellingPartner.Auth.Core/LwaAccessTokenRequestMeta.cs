using System;
using Newtonsoft.Json;

namespace Amazon.SellingPartner.Auth.Core
{
    public class LwaAccessTokenRequestMeta : IEquatable<LwaAccessTokenRequestMeta>
    {
        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        public bool Equals(LwaAccessTokenRequestMeta other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GrantType == other.GrantType && RefreshToken == other.RefreshToken && ClientId == other.ClientId && ClientSecret == other.ClientSecret && Scope == other.Scope;
        }

        public override bool Equals(object obj)
        {
            return obj is LwaAccessTokenRequestMeta other &&
                   this.GrantType == other.GrantType &&
                   this.RefreshToken == other.RefreshToken &&
                   this.ClientId == other.ClientId &&
                   this.ClientSecret == other.ClientSecret &&
                   this.Scope == other.Scope;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (GrantType != null ? GrantType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RefreshToken != null ? RefreshToken.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ClientId != null ? ClientId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ClientSecret != null ? ClientSecret.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Scope != null ? Scope.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}