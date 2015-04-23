namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using Thinktecture.IdentityServer.Core.Models;

    internal static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "api1"
                }
            };
        }
    }
}