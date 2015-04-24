using EA.Iws.Web;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Owin;
    using Services;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new ConfigurationService();

            var builder = new ContainerBuilder();
            builder.Register(c => configuration).As<ConfigurationService>().SingleInstance();
            builder.Register(c => configuration.CurrentConfiguration).As<AppConfiguration>().SingleInstance();

            var container = AutofacBootstrapper.Initialize(builder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Must register Autofac middleware FIRST!
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app, configuration.CurrentConfiguration);
        }
    }
}