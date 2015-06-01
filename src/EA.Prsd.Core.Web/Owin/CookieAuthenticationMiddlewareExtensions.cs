namespace EA.Prsd.Core.Web.Owin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Owin.Security.Cookies;
    using OAuth;
    using global::Owin;
    using Thinktecture.IdentityModel.Client;
    using ClaimTypes = Web.ClaimTypes;

    public static class CookieAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseCookieAuthentication(this IAppBuilder app, PrsdCookieAuthenticationOptions options)
        {
            return app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = options.AuthenticationType,
                LoginPath = options.LoginPath,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = context => OnValidateIdentity(context, options)
                }
            });
        }

        private static async Task OnValidateIdentity(CookieValidateIdentityContext context,
            PrsdCookieAuthenticationOptions options)
        {
            if (context.Identity == null || !context.Identity.IsAuthenticated)
            {
                return;
            }

            await UpdateAccessToken(context, options);

            await TransformClaims(context, options);
        }

        private static async Task UpdateAccessToken(CookieValidateIdentityContext context,
            PrsdCookieAuthenticationOptions options)
        {
            var expiresAt = context.Identity.FindFirst(ClaimTypes.ExpiresAt);
            if (expiresAt != null && DateTime.Parse(expiresAt.Value) < DateTime.UtcNow)
            {
                var refreshTokenClaim = context.Identity.FindFirst(OAuth2Constants.RefreshToken);
                if (refreshTokenClaim != null)
                {
                    var oauthClient = new OAuthClient(options.ApiUrl, options.ApiSecret);
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
                        var identity = response.GenerateUserIdentity(options.AuthenticationType);
                        auth.SignOut(context.Options.AuthenticationType);
                        auth.SignIn(context.Properties, identity);
                        context.ReplaceIdentity(identity);
                    }
                }
            }
        }

        private static async Task TransformClaims(CookieValidateIdentityContext context,
            PrsdCookieAuthenticationOptions options)
        {
            var claims = new List<Claim>();

            var accessTokenClaim = context.Identity.FindFirst(OAuth2Constants.AccessToken);

            if (accessTokenClaim == null)
            {
                context.RejectIdentity();
                context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                return;
            }

            var userInfoClient = new UserInfoClient(
                new Uri(options.ApiUrl + "/connect/userinfo"),
                accessTokenClaim.Value);

            var userInfo = await userInfoClient.GetAsync();

            if (userInfo.IsError || userInfo.IsHttpError)
            {
                context.RejectIdentity();
                context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                return;
            }

            userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

            claims.Add(accessTokenClaim);
            claims.Add(context.Identity.FindFirst(OAuth2Constants.RefreshToken));
            claims.Add(context.Identity.FindFirst(ClaimTypes.ExpiresAt));

            var nameId = context.Identity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (nameId != null)
            {
                claims.Add(nameId);
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaims(claims);

            context.ReplaceIdentity(id);
        }
    }
}