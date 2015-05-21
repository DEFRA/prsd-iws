namespace EA.Prsd.Core.Web.Extensions
{
    using System;
    using System.Security.Claims;
    using Thinktecture.IdentityModel.Client;
    using ClaimTypes = Web.ClaimTypes;

    public static class TokenResponseExtensions
    {
        public static ClaimsIdentity GenerateUserIdentity(this TokenResponse response, string authenticationType)
        {
            var identity = new ClaimsIdentity(authenticationType);
            identity.AddClaim(new Claim(OAuth2Constants.AccessToken, response.AccessToken));

            if (response.RefreshToken != null)
            {
                identity.AddClaim(new Claim(OAuth2Constants.RefreshToken, response.RefreshToken));
            }

            identity.AddClaim(new Claim(ClaimTypes.ExpiresAt,
                DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn).ToString()));

            return identity;
        }
    }
}