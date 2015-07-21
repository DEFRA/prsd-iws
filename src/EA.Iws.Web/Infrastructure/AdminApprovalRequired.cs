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
            bool isAdmin = identity.HasClaim(ClaimTypes.Role, "admin");
            bool isPending = identity.HasClaim(Requests.ClaimTypes.InternalUserStatus,
                InternalUserStatus.Pending.ToString());

            //TODO: add approval functionality
            if (isAdmin && isPending)
            {
                filterContext.Result = new RedirectResult("~/Admin/Registration/AwaitApproval");
            }
        }
    }
}