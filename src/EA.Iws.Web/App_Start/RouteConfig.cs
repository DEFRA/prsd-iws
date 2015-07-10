namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Controllers;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Error403", "errors/403",
                new { controller = "Errors", action = "AccessDenied" });

            routes.MapRoute("Error404", "errors/404",
                new { controller = "Errors", action = "NotFound" });

            routes.MapRoute("Error500", "errors/500",
                new { controller = "Errors", action = "InternalError" });

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}