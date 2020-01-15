namespace EA.Prsd.Core.Web.OAuth
{
    using Extensions;
    using IdentityModel.Client;
    using System;
    using System.Threading.Tasks;
    
    public class OAuthClient : IOAuthClient
    {
        private readonly TokenClient oauth2Client;

        public OAuthClient(string baseUrl, string clientId, string clientSecret)
        {
            var baseUri = new Uri(baseUrl.EnsureTrailingSlash());
            var address = new Uri(baseUri, "connect/token/");

            oauth2Client = new TokenClient(address.ToString(), clientId, clientSecret);
        }

        public async Task<TokenResponse> GetAccessTokenAsync(string username, string password)
        {
            return await oauth2Client.RequestResourceOwnerPasswordAsync(username, password,
                "openid api1 api2 all_claims profile offline_access");
        }

        public async Task<TokenResponse> GetRefreshTokenAsync(string refreshToken)
        {
            return await oauth2Client.RequestRefreshTokenAsync(refreshToken);
        }
    }
}