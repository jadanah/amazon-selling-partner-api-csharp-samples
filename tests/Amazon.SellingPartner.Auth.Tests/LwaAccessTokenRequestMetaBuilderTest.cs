using System;
using System.Collections.Generic;
using Amazon.SellingPartner.Auth.Core;
using Xunit;

namespace Amazon.SellingPartner.Auth.Tests
{
    public class LwaAccessTokenRequestMetaBuilderTest
    {
        private const string TestClientId = "cid";
        private const string TestClientSecret = "csecret";
        private const string TestRefreshToken = "rtoken";
        private static readonly Uri TestUri = new Uri("https://www.amazon.com");
        private LwaAccessTokenRequestMetaBuilder lwaAccessTokenRequestMetaBuilderUnderTest;

        public LwaAccessTokenRequestMetaBuilderTest()
        {
            lwaAccessTokenRequestMetaBuilderUnderTest = new LwaAccessTokenRequestMetaBuilder();
        }

        [Fact]
        public void LWAAuthorizationCredentialsWithoutScopesBuildsSellerTokenRequestMeta()
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = new LwaAuthorizationCredentials()
            {
                ClientId = TestClientId,
                ClientSecret = TestClientSecret,
                Endpoint = TestUri,
                RefreshToken = TestRefreshToken
            };

            LwaAccessTokenRequestMeta expected = new LwaAccessTokenRequestMeta()
            {
                ClientId = TestClientId,
                ClientSecret = TestClientSecret,
                GrantType = LwaAccessTokenRequestMetaBuilder.SellerApiGrantType,
                RefreshToken = TestRefreshToken,
                Scope = null
            };

            LwaAccessTokenRequestMeta actual = lwaAccessTokenRequestMetaBuilderUnderTest.Build(lwaAuthorizationCredentials);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LWAAuthorizationCredentialsWithScopesBuildsSellerlessTokenRequestMeta()
        {
            LwaAuthorizationCredentials lwaAuthorizationCredentials = new LwaAuthorizationCredentials()
            {
                ClientId = TestClientId,
                ClientSecret = TestClientSecret,
                Endpoint = TestUri,
                Scopes = new List<string>() { ScopeConstants.ScopeMigrationApi, ScopeConstants.ScopeNotificationsApi }
            };

            LwaAccessTokenRequestMeta expected = new LwaAccessTokenRequestMeta()
            {
                ClientId = TestClientId,
                ClientSecret = TestClientSecret,
                GrantType = LwaAccessTokenRequestMetaBuilder.SellerlessApiGrantType,
                Scope = string.Format("{0} {1}", ScopeConstants.ScopeMigrationApi, ScopeConstants.ScopeNotificationsApi),
                RefreshToken = null
            };

            LwaAccessTokenRequestMeta actual = lwaAccessTokenRequestMetaBuilderUnderTest.Build(lwaAuthorizationCredentials);

            Assert.Equal(expected, actual);
        }
    }
}