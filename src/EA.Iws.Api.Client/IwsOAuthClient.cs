namespace EA.Iws.Api.Client
{
    using System;
    using System.Threading.Tasks;
    using Thinktecture.IdentityModel.Client;

    public class IwsOAuthClient : IIwsOAuthClient
    {
        private readonly OAuth2Client oauth2Client;

        public IwsOAuthClient(string baseUrl, string clientSecret)
        {
            var baseUri = new Uri(baseUrl);

            oauth2Client = new OAuth2Client(new Uri(baseUri, "/connect/token/"), "iws", clientSecret);
        }

        public async Task<Entities.TokenResponse> GetAccessTokenAsync(string username, string password)
        {
            var result = await oauth2Client.RequestResourceOwnerPasswordAsync(username, password, "openid api1 all_claims profile");
            return new Entities.TokenResponse(result.AccessToken, result.IdentityToken, result.Error, result.ExpiresIn, result.TokenType, result.RefreshToken); 
        }
    }
}