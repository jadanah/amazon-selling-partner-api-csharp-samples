# C# - Amazon Selling Partner API Usage Samples

This repository contains examples of how to use the new Amazon Selling Partner API using *[NSwag](https://github.com/RicoSuter/NSwag)* generated clients.

I have refactored the existing example from *[Amazon](https://github.com/amzn/selling-partner-api-models/tree/main/clients/sellingpartner-api-aa-csharp)* to use HttpClient vs RestSharp with improved DI support and optional token caching.

Follow the *[Selling Partner API Guide](https://developer-docs.amazon.com/sp-api/)* in order to obtain your credentials.

Then use the examples below to get started.

Useful overview video on the sp api workflow can be found on *[Amazon Seller University](https://www.youtube.com/watch?v=MxmmoSfxSRU&list=PLyrrqKCT7jFKENJO9n_Y68-5o2GZLgLUU)* 

## Example usage using microsoft dependency injection

```c#
// get credentials
var awsKey = "AWS_KEY";
var awsSecret = "AWS_SECRET";
var roleArn = "arn:aws:iam::1234:role/ROLE_NAME";
var clientId = "amzn1.application-oa2-client.xyz";
var clientSecret = "CLIENT_SECRET";
var refreshToken = "REFRESH_TOKEN";

var spEndpoint = AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
var lwaEndpoint = EndpointConstants.LwaToken;
var region = RegionEndpoint.EUWest1;

// add services
var services = new ServiceCollection();

// add memory cache for token (if required)
services.AddMemoryCache();

// add message handlers to sign requests
services.AddTransient<AmazonSpAccessTokenHandler>();
services.AddTransient<AmazonSpSecurityTokenHandler>();

// add security token resolver (used by message handlers)
services.AddScoped<IAmazonSecurityTokenCredentialResolver>(sp => new MemoryCacheAmazonSecurityTokenCredentialResolver(sp.GetRequiredService<IMemoryCache>(), spEndpoint, roleArn, awsKey, awsSecret, region));

// add lwa client to obtain access token (used by message handlers)
services.AddHttpClient("amzn-lwa", httpClient => { httpClient.BaseAddress = new Uri(lwaEndpoint); });
services.AddScoped<ILwaClient>(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var lwaCredentials = new LwaAuthorizationCredentials
    {
        ClientId = clientId,
        ClientSecret = clientSecret,
        RefreshToken = refreshToken,
        Endpoint = new Uri(lwaEndpoint),
        Scopes = null
    };
    return new MemoryCacheHttpLwaClient(clientFactory.CreateClient("amzn-lwa"), lwaCredentials, sp.GetRequiredService<IMemoryCache>());
});

// add amazon sp api clients
// order 
services.AddHttpClient<IAmazonSellingPartnerOrdersClient, AmazonSellingPartnerOrdersClient>(client =>
    {
        client.BaseAddress = new Uri(spEndpoint);
        return new AmazonSellingPartnerOrdersClient(client)
        {
            JsonSerializerSettings =
            {
                // needed if some required fields are not going to be returned due to PII restrictions
                ContractResolver = new AmazonSellingPartnerSafeContractResolver()
            }
        };
    }).AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
    .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();

// sales
services.AddHttpClient<IAmazonSellingPartnerSalesClient, AmazonSellingPartnerSalesClient>(client =>
    {
        client.BaseAddress = new Uri(spEndpoint);
        return new AmazonSellingPartnerSalesClient(client);
    }).AddHttpMessageHandler<AmazonSpAccessTokenHandler>()
    .AddHttpMessageHandler<AmazonSpSecurityTokenHandler>();

// build service provider
var serviceProvider = services.BuildServiceProvider();

// resolve services and call api
var salesClient = serviceProvider.GetRequiredService<IAmazonSellingPartnerSalesClient>();
var ordersClient = serviceProvider.GetRequiredService<IAmazonSellingPartnerOrdersClient>();

var startDate = new DateTime(2022, 03, 01, 00, 00, 00, DateTimeKind.Utc);
var endDate = new DateTime(2022, 03, 07, 00, 00, 00, DateTimeKind.Utc);

var salesResponse = await salesClient.GetOrderMetricsAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, AmazonDateUtil.ConvertToString(startDate, endDate), Granularity.Total);

// access token and security from 1st call will be cached and used again for the 2nd request
var ordersResponse = await ordersClient.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: startDate.ToAmazonDateTimeString(),
    createdBefore: endDate.ToAmazonDateTimeString());
```

## Example usage without dependency injection

```c#
// get credentials
var awsKey = "AWS_KEY";
var awsSecret = "AWS_SECRET";
var roleArn = "arn:aws:iam::1234:role/ROLE_NAME";
var clientId = "amzn1.application-oa2-client.xyz";
var clientSecret = "CLIENT_SECRET";
var refreshToken = "REFRESH_TOKEN";

var spEndpoint = AmazonSpApiEndpoint.Europe.Endpoint.EnsureTrailingSlash();
var lwaEndpoint = EndpointConstants.LwaToken;
var region = RegionEndpoint.EUWest1;

IAmazonSecurityTokenCredentialResolver securityTokenResolver = new AmazonSecurityTokenCredentialResolver(spEndpoint, roleArn, awsKey, awsSecret, region);

var lwaAuthorizationCredentials = new LwaAuthorizationCredentials
{
    ClientId = clientId,
    ClientSecret = clientSecret,
    RefreshToken = refreshToken,
    Endpoint = new Uri(lwaEndpoint),
    Scopes = null
};

ILwaClient lwaClient = new HttpLwaClient(lwaAuthorizationCredentials);

AmazonSpAccessTokenHandler pipeline = new AmazonSpAccessTokenHandler(lwaClient)
{
    InnerHandler = new AmazonSpSecurityTokenHandler(securityTokenResolver)
    {
        InnerHandler = new HttpClientHandler()
    }
};

var httpClient = new HttpClient(pipeline)
{
    BaseAddress = new Uri(spEndpoint)
};
IAmazonSellingPartnerOrdersClient ordersClient = new AmazonSellingPartnerOrdersClient(httpClient)
{
    JsonSerializerSettings =
    {
        // needed if some required fields are not going to be returned due to PII restrictions
        ContractResolver = new AmazonSellingPartnerSafeContractResolver()
    }
};

var startDate = new DateTime(2022, 03, 01, 00, 00, 00, DateTimeKind.Utc);
var endDate = new DateTime(2022, 03, 07, 00, 00, 00, DateTimeKind.Utc);

var ordersResponse = await ordersClient.GetOrdersAsync(new[] { AmazonMarketplace.UK.MarketplaceId }, createdAfter: startDate.ToAmazonDateTimeString(),
    createdBefore: endDate.ToAmazonDateTimeString());
```

## Tests

Navigate to the `Amazon.SellingPartner.IntegrationTests` directory and initialize the user-secret store.

```sh
dotnet user-secrets init
```

Now you can add your credentials which will be used by the integration tests.

```sh
dotnet user-secrets set "Amzn:AWSKey" "AWS_KEY"
dotnet user-secrets set "Amzn:AWSSecret" "AWS_SECRET"
dotnet user-secrets set "Amzn:ClientId" "amzn1.application-oa2-client.xyz"
dotnet user-secrets set "Amzn:ClientSecret" "CLIENT_SECRET"
dotnet user-secrets set "Amzn:RefreshToken" "REFRESH_TOKEN"
dotnet user-secrets set "Amzn:RoleARN" "arn:aws:iam::1234:role/ROLE_NAME"
```