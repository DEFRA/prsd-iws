namespace EA.Iws.Api.Client
{
    using System;
    using System.Threading.Tasks;
    using Thinktecture.IdentityModel.Client;

    public class IwsOAuthClient
    {
        private readonly OAuth2Client oauth2Client;

        public IwsOAuthClient(string baseUrl, string clientSecret)
        {
            var baseUri = new Uri(baseUrl);

            oauth2Client = new OAuth2Client(new Uri(baseUri, "/connect/token/"), "carbon", clientSecret);
        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var result = await oauth2Client.RequestResourceOwnerPasswordAsync(username, password, "api1");
            return result.AccessToken;
        }
    }
}