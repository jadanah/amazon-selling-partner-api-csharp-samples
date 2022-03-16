using System.Threading.Tasks;

namespace Amazon.SellingPartner.Auth.Core
{
    public interface ILwaClient
    {
        Task<LwaTokenResponse> GetTokenResponseAsync();
        Task<string> GetAccessTokenAsync();
    }
}