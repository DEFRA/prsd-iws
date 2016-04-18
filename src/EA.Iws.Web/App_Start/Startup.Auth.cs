namespace EA.Iws.Web
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Prsd.Core;
    using Prsd.Core.Web;
    using Prsd.Core.Web.Mvc.Owin;
    using Services;
    using Constants = Infrastructure.Constants;

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
            CheckAccessToken(context);

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

        private static void CheckAccessToken(CookieValidateIdentityContext context)
        {
            var expiresAt = context.Identity.FindFirst(ClaimTypes.ExpiresAt);
            if (expiresAt != null)
            {
                DateTime expiryDate;

                if (!DateTime.TryParseExact(expiresAt.Value, "u", CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal, out expiryDate))
                {
                    // If the expiry date can't be parsed then sign the user out.
                    RejectIdentity(context);
                    return;
                }

                if (expiryDate < SystemTime.UtcNow)
                {
                    RejectIdentity(context);
                    return;
                }
            }
        }

        private static void RejectIdentity(CookieValidateIdentityContext context)
        {
            context.RejectIdentity();
            context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
        }
    }
}