using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Amazon.SellingPartner.Serialization.NewtonsoftJson
{
    /// <summary>
    /// Some field are marked as required however may be not included in response due to GDPR
    /// </summary>
    public class AmazonSellingPartnerSafeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty? jsonProp = base.CreateProperty(member, memberSerialization);
            jsonProp.Required = Required.Default;
            return jsonProp;
        }
    }
}