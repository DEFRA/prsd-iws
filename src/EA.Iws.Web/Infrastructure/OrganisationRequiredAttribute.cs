namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;
    using ClaimTypes = Core.Shared.ClaimTypes;

    public class OrganisationRequiredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.SkipAuthorisation())
            {
                return;
            }

            var principal = (ClaimsPrincipal)filterContext.HttpContext.User;
            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;

            if (!identity.IsAuthenticated)
            {
                return;
            }

            var organisationRegistered = identity.HasClaim(c => c.Type.Equals(ClaimTypes.OrganisationId));

            if (principal.IsInternalUser())
            {
                return;
            }

            if (!organisationRegistered)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Registration", action = "CreateNewOrganisation", area = string.Empty }));
            }
        }
    }
}