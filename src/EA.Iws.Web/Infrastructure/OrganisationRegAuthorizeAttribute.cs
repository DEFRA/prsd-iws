    namespace EA.Iws.Web.Infrastructure
    {
        using System.Linq;
        using System.Security.Claims;
        using System.Threading;
        using System.Web.Mvc;
        using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

        public class OrganisationRegAuthorizeAttribute : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                     || filterContext.ActionDescriptor.ActionName.Equals("CreateNewOrganisation")
                                     || filterContext.ActionDescriptor.ActionName.Equals("SelectOrganisation");

                if (skipAuthorization)
                {
                    return;
                }

                var identity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                var organisationRegistered = identity.Claims.Any(c => c.Type.Equals(Requests.ClaimTypes.OrganisationId));

                if (!organisationRegistered)
                {
                    filterContext.Result = new RedirectResult("/Registration/CreateNewOrganisation");
                }
            }
        }
    }