using Amazon.SellingPartner.Feed.Client;
using Amazon.SellingPartner.Finances.Client;
using Amazon.SellingPartner.FulfillmentInbound.Client;
using Amazon.SellingPartner.IntegrationTests.Helpers.HttpClient;
using Amazon.SellingPartner.Orders.Client;
using Amazon.SellingPartner.ProductFees.Client;
using Amazon.SellingPartner.Reports.Client;
using Amazon.SellingPartner.Sales.Client;
using Amazon.SellingPartner.Serialization.NewtonsoftJson;

namespace Amazon.SellingPartner.IntegrationTests.Helpers
{
    public class SafeSellingPartnerClientFactory : SellingPartnerClientFactory
    {
        private readonly TestAmazonSpHttpClientFactory _httpClientFactory;

        public SafeSellingPartnerClientFactory()
        {
            _httpClientFactory = new TestAmazonSpHttpClientFactory();
        }

        public override IAmazonSellingPartnerFinancesClient CreateFinancesClient()
        {
            return new AmazonSellingPartnerFinancesClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerFulfillmentInboundClient CreateFulfillmentInboundClient()
        {
            return new AmazonSellingPartnerFulfillmentInboundClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerOrdersClient CreateOrdersClient()
        {
            return new AmazonSellingPartnerOrdersClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerProductFeesClient CreateProductFeesClient()
        {
            return new AmazonSellingPartnerProductFeesClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerReportsClient CreateReportsClient()
        {
            return new AmazonSellingPartnerReportsClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerSalesClient CreateSalesClient()
        {
            return new AmazonSellingPartnerSalesClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }

        public override IAmazonSellingPartnerFeedClient CreateFeedClient()
        {
            return new AmazonSellingPartnerFeedClient(_httpClientFactory.Create())
            {
                JsonSerializerSettings =
                {
                    ContractResolver = new AmazonSellingPartnerSafeContractResolver()
                }
            };
        }
    }
}