namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using Thinktecture.IdentityModel.Client;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;
    using ClaimTypes = Requests.ClaimTypes;

    public class OrganisationRequiredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.SkipAuthorisation())
            {
                return;
            }

            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            var organisationRegistered = identity.HasClaim(c => c.Type.Equals(ClaimTypes.OrganisationId));

            bool hasRoleClaim = identity.HasClaim(c => c.Type.Equals(System.Security.Claims.ClaimTypes.Role));
            bool isAdmin = hasRoleClaim && identity.Claims.Single(c => c.Type.Equals(System.Security.Claims.ClaimTypes.Role)).Value.Equals("admin", StringComparison.InvariantCultureIgnoreCase);

            if (isAdmin)
            {
                return;
            }

            if (!organisationRegistered)
            {
                filterContext.Result = new RedirectResult("~/Registration/CreateNewOrganisation");
            }
        }
    }
}