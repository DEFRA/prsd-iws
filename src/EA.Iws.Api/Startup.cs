using EA.Iws.Api;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using IdSrv;
    using Owin;
    using Thinktecture.IdentityServer.AccessTokenValidation;
    using Thinktecture.IdentityServer.Core.Configuration;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = Factory.Configure();

            var options = new IdentityServerOptions
            {
                Factory = factory
            };

            app.UseIdentityServer(options);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44301",
                RequiredScopes = new[] { "api1" }
            });

            // Web API configuration and services
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            // Autofac
            var containerBuilder = new ContainerBuilder();
            var container = AutofacBootstrapper.Initialize(containerBuilder, config);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}