namespace EA.Iws.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using Api.Client;
    using Infrastructure;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Services;
    using Thinktecture.IdentityModel.Client;

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

            app.UseClaimsTransformation(incoming => TransformClaims(incoming, config));
        }

        private async Task OnValidateIdentity(CookieValidateIdentityContext context, AppConfiguration config)
        {
            if (context.Identity == null || !context.Identity.IsAuthenticated)
            {
                return;
            }

            var expiresAt = context.Identity.FindFirst(IwsClaimTypes.ExpiresAt);
            if (expiresAt != null && DateTime.Parse(expiresAt.Value) < DateTime.UtcNow)
            {
                var refreshTokenClaim = context.Identity.FindFirst(OAuth2Constants.RefreshToken);
                if (refreshTokenClaim != null)
                {
                    var oauthClient = new IwsOAuthClient(config.ApiUrl, config.ApiSecret);
                    var response = await oauthClient.GetRefreshTokenAsync(refreshTokenClaim.Value);
                    var auth = HttpContext.Current.GetOwinContext().Authentication;

                    if (response.IsError)
                    {
                        auth.SignOut(Constants.IwsAuthType);
                    }
                    else
                    {
                        bool isPersistent = false;

                        // TODO - get previous value for IsPersistent
                        //var authContext = await auth.AuthenticateAsync(Constants.IwsAuthType);
                        //if (authContext != null)
                        //{
                        //    var properties = authContext.Properties;
                        //    isPersistent = properties.IsPersistent;
                        //}

                        auth.SignOut(Constants.IwsAuthType);
                        auth.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, response.GenerateUserIdentity());
                    }
                }
            }
        }

        private async Task<ClaimsPrincipal> TransformClaims(ClaimsPrincipal incoming, AppConfiguration config)
        {
            if (!incoming.Identity.IsAuthenticated)
            {
                return incoming;
            }

            // parse incoming claims - create new principal with app claims
            var claims = new List<Claim>();

            var userInfoClient = new UserInfoClient(
                new Uri(config.ApiUrl + "/connect/userinfo"),
                incoming.FindFirst(OAuth2Constants.AccessToken).Value);

            var userInfo = await userInfoClient.GetAsync();
            userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

            // add access token for sample API
            claims.Add(incoming.FindFirst(OAuth2Constants.AccessToken));
            claims.Add(incoming.FindFirst(OAuth2Constants.RefreshToken));

            // keep track of access token expiration
            claims.Add(incoming.FindFirst(IwsClaimTypes.ExpiresAt));

            var nameId = incoming.FindFirst(ClaimTypes.NameIdentifier);
            if (nameId != null)
            {
                claims.Add(nameId);
            }

            var id = new ClaimsIdentity(Constants.IwsAuthType);
            id.AddClaims(claims);

            return new ClaimsPrincipal(id);
        }
    }
}