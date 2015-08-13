namespace EA.Iws.Web
{
    using System;
    using Infrastructure;
    using Microsoft.Owin;
    using Owin;
    using Prsd.Core.Web.Mvc.Owin;
    using Services;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app, AppConfiguration config)
        {
            app.UseCookieAuthentication(new PrsdCookieAuthenticationOptions(
                authenticationType: Constants.IwsAuthType)
                {
                    LoginPath = new PathString("/Account/Login"),
                    SlidingExpiration = true,
                    ExpireTimeSpan = TimeSpan.FromHours(9)
                });
        }
    }
}