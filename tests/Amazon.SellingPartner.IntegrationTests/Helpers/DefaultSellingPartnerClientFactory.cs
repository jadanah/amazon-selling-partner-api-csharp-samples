using Amazon.SellingPartner.Feed.Client;
using Amazon.SellingPartner.Finances.Client;
using Amazon.SellingPartner.FulfillmentInbound.Client;
using Amazon.SellingPartner.Orders.Client;
using Amazon.SellingPartner.ProductFees.Client;
using Amazon.SellingPartner.Reports.Client;
using Amazon.SellingPartner.Sales.Client;

namespace Amazon.SellingPartner.IntegrationTests.Helpers
{
    public class DefaultSellingPartnerClientFactory : SellingPartnerClientFactory
    {
        private readonly SellingPartnerClientFactory _safeFactory;

        public DefaultSellingPartnerClientFactory()
        {
            _safeFactory = new SafeSellingPartnerClientFactory();
        }

        public override IAmazonSellingPartnerFinancesClient CreateFinancesClient() => _safeFactory.CreateFinancesClient();

        public override IAmazonSellingPartnerFulfillmentInboundClient CreateFulfillmentInboundClient() => _safeFactory.CreateFulfillmentInboundClient();

        public override IAmazonSellingPartnerOrdersClient CreateOrdersClient() => _safeFactory.CreateOrdersClient();

        public override IAmazonSellingPartnerProductFeesClient CreateProductFeesClient() => _safeFactory.CreateProductFeesClient();

        public override IAmazonSellingPartnerReportsClient CreateReportsClient() => _safeFactory.CreateReportsClient();

        public override IAmazonSellingPartnerSalesClient CreateSalesClient() => _safeFactory.CreateSalesClient();

        public override IAmazonSellingPartnerFeedClient CreateFeedClient() => _safeFactory.CreateFeedClient();
    }
}