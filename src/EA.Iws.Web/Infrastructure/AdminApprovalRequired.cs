namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Core.Admin;
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
            bool isAdmin = identity.HasClaim(ClaimTypes.Role, "internal");
            bool isPending = identity.HasClaim(Core.Shared.ClaimTypes.InternalUserStatus,
                InternalUserStatus.Pending.ToString());

            if (isAdmin && isPending)
            {
                filterContext.Result = new RedirectResult("~/Admin/Registration/AwaitApproval");
            }
        }
    }
}