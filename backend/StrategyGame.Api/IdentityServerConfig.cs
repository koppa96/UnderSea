using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using static IdentityServer4.Models.IdentityResources;

namespace StrategyGame.Api
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new OpenId(),
                new Email(),
                new Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IConfiguration configuration)
        {
            return new List<ApiResource>
            {
                new ApiResource(configuration["ApiName"], "UnderSea API")
                {
                    UserClaims = { JwtClaimTypes.Name, JwtClaimTypes.Email }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = configuration["ClientName"],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(configuration["ClientSecret"].Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        configuration["ApiName"]
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}