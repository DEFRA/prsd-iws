﻿namespace EA.Iws.Web.Infrastructure
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class MaintenanceModeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (!filterContext.RouteData.Values["controller"].Equals("Errors") &&
                !filterContext.RouteData.Values["action"].Equals("Maintenance"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Errors", action = "Maintenance" }));
            }
        }
    }
}