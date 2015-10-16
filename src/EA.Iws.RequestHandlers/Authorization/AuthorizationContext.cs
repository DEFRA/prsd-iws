namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Security.Claims;
    using Prsd.Core;

    public class AuthorizationContext
    {
        public string Name { get; private set; }

        public ClaimsPrincipal Principal { get; private set; }

        public AuthorizationContext(ClaimsPrincipal principal, string name)
        {
            Guard.ArgumentNotNull(() => name, name);
            Guard.ArgumentNotNull(() => principal, principal);

            Name = name;
            Principal = principal;
        }
    }
}
