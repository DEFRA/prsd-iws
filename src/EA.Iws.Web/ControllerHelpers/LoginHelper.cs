namespace EA.Iws.Web.ControllerHelpers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Api.Client;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Thinktecture.IdentityModel.Client;
    using TokenResponse = Api.Client.Entities.TokenResponse;

    public static class LoginHelper
    {
        public static async Task LogIn(string email, string password, Func<IwsOAuthClient> oauthClient,
            IAuthenticationManager authenticationManager, bool isPersistent = false)
        {
            var signInResponse = await oauthClient().GetAccessTokenAsync(email, password);
            var identity = GenerateUserIdentity(signInResponse);
            authenticationManager.SignIn(new AuthenticationProperties(), identity);
        }

        private static ClaimsIdentity GenerateUserIdentity(TokenResponse response)
        {
            var identity = new ClaimsIdentity(Constants.IwsAuthType);
            identity.AddClaim(new Claim(OAuth2Constants.AccessToken, response.AccessToken));
            if (response.IdentityToken != null)
            {
                identity.AddClaim(new Claim(OAuth2Constants.IdentityToken, response.IdentityToken));
            }
            identity.AddClaim(new Claim(IwsClaimTypes.ExpiresAt,
                DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToString()));
            return identity;
        }
    }
}