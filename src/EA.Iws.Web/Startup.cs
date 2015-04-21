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
            var containerBuilder = new ContainerBuilder();

            var container = AutofacBootstrapper.Initialize(containerBuilder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Must register Autofac middleware FIRST!
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            using (var context = container.BeginLifetimeScope())
            {
                var config = context.Resolve<AppConfiguration>();
                ConfigureAuth(app, config);
            }
        }
    }
}