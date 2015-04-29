namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Security.Claims;
    using Thinktecture.IdentityModel.Client;
    using TokenResponse = Api.Client.Entities.TokenResponse;

    public static class TokenResponseExtensions
    {
        public static ClaimsIdentity GenerateUserIdentity(this TokenResponse response)
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