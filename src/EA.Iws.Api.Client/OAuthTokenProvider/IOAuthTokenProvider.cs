namespace EA.Iws.Api.Client.OAuthTokenProvider
{
    using System.Threading.Tasks;

    public interface IOAuthTokenProvider
    {
        Task<string> GetAccessTokenAsync();
    }
}
