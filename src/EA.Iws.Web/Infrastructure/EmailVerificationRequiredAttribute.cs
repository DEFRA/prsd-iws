namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Thinktecture.IdentityModel.Client;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

    public class EmailVerificationRequiredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.SkipAuthorisation())
            {
                return;
            }

            var principal = (ClaimsPrincipal)filterContext.HttpContext.User;
            var identity = (ClaimsIdentity)principal.Identity;

            var hasEmailVerifiedClaim = identity.HasClaim(c => c.Type.Equals(JwtClaimTypes.EmailVerified));

            if (hasEmailVerifiedClaim && identity.Claims.Any(c => 
                    c.Type.Equals(JwtClaimTypes.EmailVerified) 
                    && c.Value.Equals("false", StringComparison.InvariantCultureIgnoreCase)))                
            {
                var redirectAddress = principal.IsInternalUser() 
                    ? new RouteValueDictionary(new { controller = "Registration", action = "AdminEmailVerificationRequired", area = "Admin" })
                    : new RouteValueDictionary(new { controller = "Account", action = "EmailVerificationRequired", area = string.Empty });

                filterContext.Result = new RedirectToRouteResult(redirectAddress);
            }
        }
    }
}