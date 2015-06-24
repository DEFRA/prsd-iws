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

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}