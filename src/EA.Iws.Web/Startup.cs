using EA.Iws.Web;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EA.Iws.Web
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using IdentityModel;
    using Infrastructure;
    using Owin;
    using Services;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;

    public partial class Startup
    {
        public static string ApplicationVersion { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            var configuration = new ConfigurationService();
            var auditService = new AuditService();

            var builder = new ContainerBuilder();
            builder.Register(c => configuration).As<ConfigurationService>().SingleInstance();
            builder.Register(c => configuration.CurrentConfiguration).As<AppConfiguration>().SingleInstance();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => auditService).As<IAuditService>().SingleInstance();

            var container = AutofacBootstrapper.Initialize(builder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Must register Autofac middleware FIRST!
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app, configuration.CurrentConfiguration);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = JwtClaimTypes.Subject;
            AntiForgeryConfig.RequireSsl = true;
            AntiForgeryConfig.CookieName = Prsd.Core.Web.Constants.CookiePrefix + Constants.AntiForgeryCookieName;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, configuration.CurrentConfiguration);

            MvcHandler.DisableMvcResponseHeader = true;

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}