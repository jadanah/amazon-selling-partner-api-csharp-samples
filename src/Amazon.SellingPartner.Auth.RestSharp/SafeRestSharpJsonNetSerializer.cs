using Amazon.SellingPartner.Serialization.NewtonsoftJson;
using RestSharp.Serializers.NewtonsoftJson;

namespace Amazon.SellingPartner.Auth.RestSharp
{
    public class SafeRestSharpJsonNetSerializer : JsonNetSerializer
    {
        public SafeRestSharpJsonNetSerializer()
        {
            DefaultSettings.ContractResolver = new AmazonSellingPartnerSafeContractResolver();
        }
    }
}