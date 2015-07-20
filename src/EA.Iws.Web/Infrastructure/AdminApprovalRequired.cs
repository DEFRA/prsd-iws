namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

    public class AdminApprovalRequired : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.SkipAuthorisation())
            {
                return;
            }

            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            bool hasRoleClaim = identity.HasClaim(c => c.Type.Equals(ClaimTypes.Role));
            bool isAdmin = hasRoleClaim && identity.Claims.Single(c => c.Type.Equals(ClaimTypes.Role)).Value.Equals("admin", StringComparison.InvariantCultureIgnoreCase);

            //TODO: add approval functionality
            if (isAdmin)
            {
                filterContext.Result = new RedirectResult("~/Admin/Registration/AwaitApproval");
            }
        }
    }
}