namespace Amazon.SellingPartner.Auth.Core
{
    public interface IRateLimitConfiguration
    {
        int GetRateLimitPermit();
        int GetTimeOut();
    }
}