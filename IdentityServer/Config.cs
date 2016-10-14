using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityServer
{
    public static class Config
    {
        public static Client[] Clients = new Client[]
        {
            new Client
            {
                ClientId = "client",
                ClientName = "test client",
                Enabled = true,
                Flow = Flows.Custom,
                AllowedCustomGrantTypes = new List<string>
                {
                    "windows"
                },
                AllowedScopes = new List<string>
                {
                    "openid", "profile", "roles", "api"
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256()),
                },
                RequireConsent = false,
                AccessTokenType = AccessTokenType.Jwt
            }
        };

        public static Scope[] Scopes = new Scope[]
        {
            StandardScopes.OpenId,
            StandardScopes.ProfileAlwaysInclude,
            StandardScopes.RolesAlwaysInclude,
            new Scope
            {
                Name = "api",
                DisplayName = "resource api",
                Type = ScopeType.Resource,
                Claims = new List<ScopeClaim>()
                {
                    new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Name)
                }
            }
        };

    }
}