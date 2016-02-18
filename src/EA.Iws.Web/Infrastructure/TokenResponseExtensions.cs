namespace EA.Iws.Web.Infrastructure
{
    using System.Security.Claims;
    using IdentityModel.Client;
    using Prsd.Core.Web.Extensions;

    public static class TokenResponseExtensions
    {
        public static ClaimsIdentity GenerateUserIdentity(this TokenResponse response)
        {
            return response.GenerateUserIdentity(Constants.IwsAuthType);
        }
    }
}