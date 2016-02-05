namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Security.Claims;
    using Prsd.Core;

    public class AuthorizationContext
    {
        public string Activity { get; private set; }

        public ClaimsPrincipal Principal { get; private set; }

        public AuthorizationContext(ClaimsPrincipal principal, string activity)
        {
            Guard.ArgumentNotNull(() => activity, activity);
            Guard.ArgumentNotNull(() => principal, principal);

            Activity = activity;
            Principal = principal;
        }
    }
}
