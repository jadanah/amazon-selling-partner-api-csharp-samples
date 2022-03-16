using System;

namespace Amazon.SellingPartner.Auth.Core
{
    public interface IDateHelper
    {
        DateTime GetUtcNow();
    }
}