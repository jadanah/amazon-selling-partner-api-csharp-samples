using System;

namespace Amazon.SellingPartner.Auth.Core
{
    public class SigningDateHelper : IDateHelper
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}