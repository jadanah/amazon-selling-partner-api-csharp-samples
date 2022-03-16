namespace Amazon.SellingPartner.Core
{
    /// <summary>
    /// https://developer-docs.amazon.com/sp-api/docs/marketplace-ids
    /// </summary>
    public sealed class AmazonMarketplace
    {
        // North America
        public static readonly AmazonMarketplace Canada = new AmazonMarketplace("North America", "Canada", "CA", "A2EUQ1WTGCTBG2");
        public static readonly AmazonMarketplace USA = new AmazonMarketplace("North America", "United States of America", "US", "ATVPDKIKX0DER");
        public static readonly AmazonMarketplace Mexico = new AmazonMarketplace("North America", "Mexico", "MX", "A1AM78C64UM0Y8");
        public static readonly AmazonMarketplace Brazil = new AmazonMarketplace("North America", "Brazil", "BR", "A2Q3Y263D00KWC");

        // Europe
        public static readonly AmazonMarketplace Spain = new AmazonMarketplace("Europe", "Spain", "ES", "A1RKKUPIHCS9HS");
        public static readonly AmazonMarketplace UK = new AmazonMarketplace("Europe", "United Kingdom", "UK", "A1F83G8C2ARO7P");
        public static readonly AmazonMarketplace France = new AmazonMarketplace("Europe", "France", "FR", "A13V1IB3VIYZZH");
        public static readonly AmazonMarketplace Netherlands = new AmazonMarketplace("Europe", "Netherlands", "NL", "A1805IZSGTT6HS");
        public static readonly AmazonMarketplace Germany = new AmazonMarketplace("Europe", "Germany", "DE", "A1PA6795UKMFR9");
        public static readonly AmazonMarketplace Italy = new AmazonMarketplace("Europe", "Italy", "IT", "APJ6JRA9NG5V4");
        public static readonly AmazonMarketplace Sweden = new AmazonMarketplace("Europe", "Sweden", "SE", "A2NODRKZP88ZB9");
        public static readonly AmazonMarketplace Poland = new AmazonMarketplace("Europe", "Poland", "PL", "A1C3SOZRARQ6R3");
        public static readonly AmazonMarketplace Egypt = new AmazonMarketplace("Europe", "Egypt", "EG", "ARBP9OOSHTCHU");
        public static readonly AmazonMarketplace Turkey = new AmazonMarketplace("Europe", "Turkey", "TR", "A33AVAJ2PDY3EV");
        public static readonly AmazonMarketplace SaudiArabia = new AmazonMarketplace("Europe", "Saudi Arabia", "SA", "A17E79C6D8DWNP");
        public static readonly AmazonMarketplace UAE = new AmazonMarketplace("Europe", "United Arab Emirates", "AE", "A2VIGQ35RCS4UG");
        public static readonly AmazonMarketplace India = new AmazonMarketplace("Europe", "India", "IN", "A21TJRUUN4KGV");

        // Far East
        public static readonly AmazonMarketplace Singapore = new AmazonMarketplace("Far East", "Singapore", "SG", "A19VAU5U5O7RUS");
        public static readonly AmazonMarketplace Australia = new AmazonMarketplace("Far East", "Australia", "AU", "A39IBJ37TRP1C6");
        public static readonly AmazonMarketplace Japan = new AmazonMarketplace("Far East", "Japan", "JP", "A1VC38T7YXB528");

        private AmazonMarketplace(string region, string country, string countryCode, string marketplaceId)
        {
            Region = region;
            Country = country;
            CountryCode = countryCode;
            MarketplaceId = marketplaceId;
        }

        public string Region { get; }
        public string Country { get; }
        public string CountryCode { get; }
        public string MarketplaceId { get; }
    }
}