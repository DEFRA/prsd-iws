﻿namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Models;
    using Services;
    using Client = IdentityServer3.Core.Models.Client;

    internal static class Clients
    {
        public static List<Client> Get(AppConfiguration config)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "IWS Web",
                    ClientId = config.ApiClientId,
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,
                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(config.ApiSecret.Sha256())
                    },
                    AllowAccessToAllScopes = true,
                    AccessTokenLifetime = 43200 // 12 hours
                },
                new Client
                {
                    ClientName = "IWS Web Unauthenticated",
                    ClientId = config.ApiClientCredentialId,
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(config.ApiClientCredentialSecret.Sha256())
                    },
                    AccessTokenLifetime = 10805, // 3 hour,
                    AllowAccessToAllScopes = true
                }
            };
        }
    }
}