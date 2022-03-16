namespace Amazon.SellingPartner.Auth.Core
{
    public class AwsAuthenticationCredentials
    {
        /// <summary>
        /// AWS IAM User Access Key Id
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// AWS IAM User Secret Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        ///  AWS Region
        /// </summary>
        public string Region { get; set; }
    }
}