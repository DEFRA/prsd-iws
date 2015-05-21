namespace EA.Iws.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Prsd.Core.Web.OAuth;
    using Services;
    using Thinktecture.IdentityModel.Client;
    using ClaimTypes = Prsd.Core.Web.ClaimTypes;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app, AppConfiguration config)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.IwsAuthType,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = context => OnValidateIdentity(context, config)
                }
            });
        }

        private static async Task OnValidateIdentity(CookieValidateIdentityContext context, AppConfiguration config)
        {
            if (context.Identity == null || !context.Identity.IsAuthenticated)
            {
                return;
            }

            await UpdateAccessToken(context, config);

            var id = await TransformClaims(context, config);

            if (id != null)
            {
                context.ReplaceIdentity(id);
            }
        }

        private static async Task UpdateAccessToken(CookieValidateIdentityContext context, AppConfiguration config)
        {
            var expiresAt = context.Identity.FindFirst(ClaimTypes.ExpiresAt);
            if (expiresAt != null && DateTime.Parse(expiresAt.Value) < DateTime.UtcNow)
            {
                var refreshTokenClaim = context.Identity.FindFirst(OAuth2Constants.RefreshToken);
                if (refreshTokenClaim != null)
                {
                    var oauthClient = new OAuthClient(config.ApiUrl, config.ApiSecret);
                    var response = await oauthClient.GetRefreshTokenAsync(refreshTokenClaim.Value);
                    var auth = context.OwinContext.Authentication;

                    if (response.IsError)
                    {
                        // If the refresh token doesn't work (e.g. it is expired) then sign the user out.
                        context.RejectIdentity();
                        auth.SignOut(context.Options.AuthenticationType);
                    }
                    else
                    {
                        // Create a new cookie from the token response by signing out and in.
                        var identity = response.GenerateUserIdentity();
                        auth.SignOut(context.Options.AuthenticationType);
                        auth.SignIn(context.Properties, identity);
                        context.ReplaceIdentity(identity);
                    }
                }
            }
        }

        private static async Task<ClaimsIdentity> TransformClaims(CookieValidateIdentityContext context,
            AppConfiguration config)
        {
            var claims = new List<Claim>();

            var userInfoClient = new UserInfoClient(
                new Uri(config.ApiUrl + "/connect/userinfo"),
                context.Identity.FindFirst(OAuth2Constants.AccessToken).Value);

            var userInfo = await userInfoClient.GetAsync();

            if (userInfo.IsError || userInfo.IsHttpError)
            {
                context.RejectIdentity();
                context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                return null;
            }

            userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

            claims.Add(context.Identity.FindFirst(OAuth2Constants.AccessToken));
            claims.Add(context.Identity.FindFirst(OAuth2Constants.RefreshToken));
            claims.Add(context.Identity.FindFirst(ClaimTypes.ExpiresAt));

            var nameId = context.Identity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (nameId != null)
            {
                claims.Add(nameId);
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaims(claims);
            return id;
        }
    }
}