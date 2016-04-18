namespace EA.Iws.Api.Identity
{
    using System;
    using System.Security.Claims;
    using Microsoft.Owin.Security;
    using Prsd.Core.Domain;
    using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

    public class UserContext : IUserContext
    {
        private readonly IAuthenticationManager authentication;

        public UserContext(IAuthenticationManager authentication)
        {
            this.authentication = authentication;
        }

        public Guid UserId
        {
            get
            {
                var claimsPrincipal = authentication.User;

                if (claimsPrincipal != null)
                {
                    foreach (var identity in claimsPrincipal.Identities)
                    {
                        if (identity.AuthenticationType.Equals("BEARER", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return GetUserId(identity, "sub");
                        }

                        return GetUserId(identity, ClaimTypes.NameIdentifier);
                    }
                }

                return Guid.Empty;
            }
        }

        public ClaimsPrincipal Principal
        {
            get { return authentication.User; }
        }

        private static Guid GetUserId(ClaimsIdentity identity, string claimType)
        {
            var idClaim = identity.FindFirst(claimType);

            if (idClaim != null)
            {
                return Guid.Parse(idClaim.Value);
            }

            return Guid.Empty;
        }
    }
}