namespace EA.Prsd.Core.Web.OAuth
{
    using IdentityModel.Client;
    using System.Threading.Tasks;

    public interface IOAuthClientCredentialClient
    {
        Task<TokenResponse> GetClientCredentialsAsync();
    }
}
