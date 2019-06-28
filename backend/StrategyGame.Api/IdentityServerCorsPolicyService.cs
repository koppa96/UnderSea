using IdentityServer4.Services;
using System.Threading.Tasks;

namespace StrategyGame.Api
{
    /// <summary>
    /// Provides a CORS policy service that allows any and all origins. Warning: Should not be used in production.
    /// </summary>
    public class IdentityServerCorsPolicyService : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}