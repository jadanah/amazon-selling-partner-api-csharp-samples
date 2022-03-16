using System.Threading;
using System.Threading.Tasks;

namespace Amazon.SellingPartner.Auth.Core
{
    public interface IAmazonSecurityTokenCredentialResolver
    {
        Task<AmazonSecurityTokenCredentials> GetCredentialsAsync(CancellationToken cancellationToken);
    }
}