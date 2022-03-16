using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using Amazon.SellingPartner.Orders.Client;

namespace Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient
{
    /// <summary>
    /// Derived class allowing us to override the json contract resolvers so it wont throw on missing required data
    /// </summary>
    public class SafeAmazonSellingPartnerOrdersClient : AmazonSellingPartnerOrdersClient
    {
        public SafeAmazonSellingPartnerOrdersClient(System.Net.Http.HttpClient httpClient) : base(httpClient)
        {
            JsonSerializerSettings.ContractResolver = new AmazonSellingPartnerSafeContractResolver();
        }
    }
}