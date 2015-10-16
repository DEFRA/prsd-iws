namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.Authorization;

    public class RequestAuthorizationErrorFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is RequestAuthorizationException)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 403;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                    new { action = "Login", controller = "Account", area = string.Empty }));

                filterContext.ExceptionHandled = true;
            }
        }
    }
}