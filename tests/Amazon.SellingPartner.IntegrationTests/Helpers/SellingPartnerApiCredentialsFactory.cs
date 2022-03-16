using Amazon.SellingPartner.Auth.Core;
using Microsoft.Extensions.Configuration;

namespace Amazon.SellingPartner.IntegrationTests.Helpers
{
    public class SellingPartnerApiCredentialsFactory
    {
        IConfiguration Configuration { get; set; }

        public SellingPartnerApiCredentialsFactory()
        {
            // the type specified here is just so the secrets library can 
            // find the UserSecretId we added in the csproj file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddUserSecrets<SellingPartnerApiCredentialsFactory>();

            Configuration = builder.Build();
        }


        public SellingPartnerApiCredentials CreateFromUserSecrets()
        {
            return new SellingPartnerApiCredentials(awsKey: Configuration["Amzn:AWSKey"], awsSecret: Configuration["Amzn:AWSSecret"], clientId: Configuration["Amzn:ClientId"],
                clientSecret: Configuration["Amzn:ClientSecret"], refreshToken: Configuration["Amzn:RefreshToken"], roleArn: Configuration["Amzn:RoleARN"]);

        }
    }
}