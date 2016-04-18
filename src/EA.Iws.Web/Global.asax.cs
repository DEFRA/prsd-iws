namespace EA.Iws.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Elmah;
    using Logging;

    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }

        protected void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            ElmahFilter.FilterSensitiveData(args);
        }

        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            ElmahFilter.FilterSensitiveData(args);
        }
    }
}