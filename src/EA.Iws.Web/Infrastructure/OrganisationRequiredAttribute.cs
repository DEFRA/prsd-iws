namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using AuthorizationContext = System.Web.Mvc.AuthorizationContext;
    using ClaimTypes = Requests.ClaimTypes;

    public class OrganisationRequiredAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                    || filterContext.ActionDescriptor.ActionName.Equals("CreateNewOrganisation")
                                    || filterContext.ActionDescriptor.ActionName.Equals("SelectOrganisation")
                                    || filterContext.ActionDescriptor.ActionName.Equals("LogOff")
                                    || filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Elmah");

            if (skipAuthorization)
            {
                return;
            }

            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            var organisationRegistered = identity.HasClaim(c => c.Type.Equals(ClaimTypes.OrganisationId));

            if (!organisationRegistered)
            {
                filterContext.Result = new RedirectResult("/Registration/CreateNewOrganisation");
            }
        }
    }
}