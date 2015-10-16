namespace EA.Iws.Web.Infrastructure
{
    using System.Reflection;
    using System.Security.Claims;
    using System.Web;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using ClaimTypes = Core.Shared.ClaimTypes;

    public class RequestAuthorizationChecker
    {
        public static bool CheckAccess<TResponse>(IRequest<TResponse> request, HttpContextBase httpContext)
        {
            var requestAuthorizationAttribute = request.GetType().GetCustomAttribute<RequestAuthorizationAttribute>();

            if (requestAuthorizationAttribute == null)
            {
                return true;
            }

            return UserHasRequestClaim(httpContext, requestAuthorizationAttribute.Name);
        }

        private static bool UserHasRequestClaim(HttpContextBase httpContext, string requestAttributeName)
        {
            if (httpContext == null)
            {
                return false;
            }

            var principal = httpContext.User as ClaimsPrincipal;

            if (principal == null)
            {
                return false;
            }

            return principal.HasClaim(ClaimTypes.AuthorizedRequest, requestAttributeName);
        } 
    }
}