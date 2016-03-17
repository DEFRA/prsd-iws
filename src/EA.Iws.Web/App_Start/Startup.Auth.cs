namespace EA.Iws.Web
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Prsd.Core.Web.Mvc.Owin;
    using Services;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app, AppConfiguration config)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.IwsAuthType,
                CookieName = Prsd.Core.Web.Constants.CookiePrefix + Constants.IwsAuthType,
                LoginPath = new PathString("/Account/Login"),
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = context => OnValidateIdentity(context),
                    OnApplyRedirect = context => OnApplyRedirect(context)
                }
            });
        }

        private static async Task OnValidateIdentity(CookieValidateIdentityContext context)
        {
            await IdentityValidationHelper.UpdateAccessToken(context);
            await IdentityValidationHelper.TransformClaims(context);
        }

        private static void OnApplyRedirect(CookieApplyRedirectContext context)
        {
            if (context.Request.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
        }
    }
}