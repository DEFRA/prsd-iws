namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Core.Admin;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

    public class AdminApprovalRequired : AuthorizeAttribute
    {
        private static readonly string Route = "~/Admin/Registration/";
        private static readonly string PendingAction = "AwaitApproval";
        private static readonly string RejectedAction = "ApprovalRejected";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.SkipAuthorisation())
            {
                return;
            }

            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;

            var isAdmin = identity.HasClaim(ClaimTypes.Role, "internal");

            if (!isAdmin)
            {
                return;
            }

            RedirectInternalUser(filterContext, identity);
        }

        private void RedirectInternalUser(AuthorizationContext filterContext, ClaimsIdentity identity)
        {
            var statusClaim = identity.FindFirst(Core.Shared.ClaimTypes.InternalUserStatus);

            if (statusClaim == null)
            {
                return;
            }

            var status = statusClaim.Value;

            if (status == InternalUserStatus.Pending.ToString() && filterContext.ActionDescriptor.ActionName != PendingAction)
            {
                filterContext.Result = new RedirectResult(Route + PendingAction);
            }
            else if (status == InternalUserStatus.Rejected.ToString() && filterContext.ActionDescriptor.ActionName != RejectedAction)
            {
                filterContext.Result = new RedirectResult(Route + RejectedAction);
            }
        }
    }
}