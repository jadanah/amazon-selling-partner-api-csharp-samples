using Amazon.SellingPartner.Feed.Client;
using Amazon.SellingPartner.Finances.Client;
using Amazon.SellingPartner.FulfillmentInbound.Client;
using Amazon.SellingPartner.Orders.Client;
using Amazon.SellingPartner.ProductFees.Client;
using Amazon.SellingPartner.Reports.Client;
using Amazon.SellingPartner.Sales.Client;

namespace Amazon.SellingPartner.IntegrationTests.Helpers
{
    public abstract class SellingPartnerClientFactory
    {
        public abstract IAmazonSellingPartnerFinancesClient CreateFinancesClient();
        public abstract IAmazonSellingPartnerFulfillmentInboundClient CreateFulfillmentInboundClient();
        public abstract IAmazonSellingPartnerOrdersClient CreateOrdersClient();
        public abstract IAmazonSellingPartnerProductFeesClient CreateProductFeesClient();
        public abstract IAmazonSellingPartnerReportsClient CreateReportsClient();
        public abstract IAmazonSellingPartnerSalesClient CreateSalesClient();
        public abstract IAmazonSellingPartnerFeedClient CreateFeedClient();
    }
}