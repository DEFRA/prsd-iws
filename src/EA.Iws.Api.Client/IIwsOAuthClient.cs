namespace EA.Iws.Api.Client
{
    using System.Threading.Tasks;
    using Thinktecture.IdentityModel.Client;

    public interface IIwsOAuthClient
    {
        Task<TokenResponse> GetAccessTokenAsync(string username, string password);
        Task<TokenResponse> GetRefreshTokenAsync(string refreshToken);
    }
}