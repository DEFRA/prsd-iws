namespace EA.Iws.Api.Client
{
    using System.Threading.Tasks;

    public interface IIwsOAuthClient
    {
        Task<Entities.TokenResponse> GetAccessTokenAsync(string username, string password);
    }
}