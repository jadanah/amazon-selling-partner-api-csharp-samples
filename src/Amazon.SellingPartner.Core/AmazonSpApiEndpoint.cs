using System;

namespace Amazon.SellingPartner.Core
{
    /// <summary>
    /// https://developer-docs.amazon.com/sp-api/docs/sp-api-endpoints
    /// </summary>
    public sealed class AmazonSpApiEndpoint
    {
        public static readonly AmazonSpApiEndpoint NorthAmerica = new AmazonSpApiEndpoint(region: "North America", regionDescription: "North America (Canada, US, Mexico, and Brazil marketplaces)",
            endpoint: "https://sellingpartnerapi-na.amazon.com", awsRegion: "us-east-1");

        public static readonly AmazonSpApiEndpoint Europe = new AmazonSpApiEndpoint(region: "Europe",
            regionDescription: "Europe (Spain, UK, France, Netherlands, Germany, Italy, Sweden, Poland, Saudi Arabia, Egypt, Turkey, United Arab Emirates, and India marketplaces)",
            endpoint: "https://sellingpartnerapi-eu.amazon.com", awsRegion: "eu-west-1");

        public static readonly AmazonSpApiEndpoint FarEast = new AmazonSpApiEndpoint(region: "Far East", regionDescription: "Far East (Singapore, Australia, and Japan marketplaces)",
            endpoint: "https://sellingpartnerapi-fe.amazon.com", awsRegion: "us-west-2");


        private AmazonSpApiEndpoint(string region, string regionDescription, string endpoint, string awsRegion)
        {
            Region = region ?? throw new ArgumentNullException(nameof(region));
            RegionDescription = regionDescription ?? throw new ArgumentNullException(nameof(regionDescription));
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            AwsRegion = awsRegion ?? throw new ArgumentNullException(nameof(awsRegion));
        }

        public string Region { get; }
        public string RegionDescription { get; }
        public string Endpoint { get; }
        public string AwsRegion { get; }
    }
}