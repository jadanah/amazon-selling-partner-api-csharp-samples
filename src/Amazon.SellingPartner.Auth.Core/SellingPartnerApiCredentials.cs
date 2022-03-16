namespace Amazon.SellingPartner.Auth.Core
{
    public class SellingPartnerApiCredentials
    {
        public SellingPartnerApiCredentials(string awsKey, string awsSecret, string clientId, string clientSecret, string refreshToken, string roleArn)
        {
            AWSKey = awsKey;
            AWSSecret = awsSecret;
            ClientId = clientId;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
            RoleARN = roleArn;
        }

        public string AWSKey { get; }
        public string AWSSecret { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RefreshToken { get; }
        public string RoleARN { get; }
    }
}