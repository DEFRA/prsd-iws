namespace EA.Iws.Api.Identity
{
    using System;
    using System.Security.Claims;
    using Microsoft.Owin.Security;
    using Prsd.Core.Domain;

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
                            var idClaim = identity.FindFirst("sub");

                            if (idClaim != null)
                            {
                                return Guid.Parse(idClaim.Value);
                            }
                        }
                    }
                }

                return Guid.Empty;
            }
        }

        public ClaimsPrincipal Principal
        {
            get { return authentication.User; }
        }
    }
}