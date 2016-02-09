namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Admin;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

    public class AdminApprovalRequiredAttribute : AuthorizeAttribute
    {
        private const string Area = "Admin";
        private const string Controller = "Registration";
        private const string PendingAction = "AwaitApproval";
        private const string RejectedAction = "ApprovalRejected";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.SkipAuthorisation())
            {
                return;
            }

            var principal = (ClaimsPrincipal)filterContext.HttpContext.User;

            if (!principal.IsInternalUser())
            {
                return;
            }

            RedirectInternalUser(filterContext, (ClaimsIdentity)principal.Identity);
        }

        private static void RedirectInternalUser(AuthorizationContext filterContext, ClaimsIdentity identity)
        {
            var statusClaim = identity.FindFirst(Core.Shared.ClaimTypes.InternalUserStatus);

            if (statusClaim == null)
            {
                return;
            }

            var status = statusClaim.Value;

            if (status == InternalUserStatus.Pending.ToString() && filterContext.ActionDescriptor.ActionName != PendingAction)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = Controller, action = PendingAction, area = Area }));
            }
            else if (status == InternalUserStatus.Rejected.ToString() && filterContext.ActionDescriptor.ActionName != RejectedAction)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = Controller, action = RejectedAction, area = Area }));
            }
        }
    }
}