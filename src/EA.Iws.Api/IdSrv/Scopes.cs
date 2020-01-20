namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Models;

    internal static class Scopes
    {
        public static List<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope { DisplayName = "IWS API", Name = "api1" },
                new Scope() { DisplayName = "IWS Virus Scan API", Name = "api2" },
                new Scope() { DisplayName = "IWS API Unauthenticated", Name = "api3" },
                StandardScopes.AllClaims,
                StandardScopes.OfflineAccess
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}