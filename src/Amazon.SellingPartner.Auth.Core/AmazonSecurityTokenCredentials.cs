using System;

namespace Amazon.SellingPartner.Auth.Core
{
    public class AmazonSecurityTokenCredentials
    {
        public string SessionToken { get; set; }
        public string SecretAccessKey { get; set; }
        public string AccessKeyId { get; set; }
        public DateTime? Expiration { get; set; }
        public string Region { get; set; }
        public string Host { get; set; }
    }
}