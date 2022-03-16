namespace Amazon.SellingPartner.Auth.Core
{
    public class RateLimitConfigurationOnRequests : IRateLimitConfiguration
    {
        /// <summary>
        ///  RateLimiter Permit
        /// </summary>
        private readonly int _rateLimitPermit;

        /// <summary>
        /// Timeout for RateLimiter
        /// </summary>
        private readonly int _waitTimeOutInMilliSeconds;

        public RateLimitConfigurationOnRequests(int waitTimeOutInMilliSeconds, int rateLimitPermit)
        {
            _waitTimeOutInMilliSeconds = waitTimeOutInMilliSeconds;
            _rateLimitPermit = rateLimitPermit;
        }

        public int GetRateLimitPermit()
        {
            return _rateLimitPermit;
        }

        public int GetTimeOut()
        {
            return _waitTimeOutInMilliSeconds;
        }
    }
}