namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using Services;
    using Thinktecture.IdentityServer.Core.Models;

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
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret(config.ApiSecret.Sha256())
                    }
                }
            };
        }
    }
}