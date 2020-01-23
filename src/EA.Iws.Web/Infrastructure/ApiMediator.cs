namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Threading.Tasks;
    using System.Web;
    using Api.Client;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;
    using Prsd.Core.Web.OAuth;

    internal class ApiMediator : IMediator
    {
        private readonly IIwsClient client;
        private readonly HttpContextBase httpContext;
        private readonly IOAuthClientCredentialClient oauthClientCredentialClient;

        public ApiMediator(IIwsClient client, HttpContextBase httpContext, 
            IOAuthClientCredentialClient oauthClientCredentialClient)
        {
            this.client = client;
            this.httpContext = httpContext;
            this.oauthClientCredentialClient = oauthClientCredentialClient;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var allowUnauthorized =
                request.GetType().GetCustomAttributes(typeof(AllowUnauthorizedUserAttribute)).SingleOrDefault();

            if (allowUnauthorized != null)
            {
                return await client.SendAsync(await GetAccessToken(), request).ConfigureAwait(false);
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                throw new SecurityException("Unauthenticated user");
            }

            var accessToken = await GetAccessToken();
            return await client.SendAsync(accessToken, request).ConfigureAwait(false);
        }

        public Task<object> SendAsync(object request, Type responseType)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetAccessToken()
        {
            var token = httpContext.User.GetAccessToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                var tokenResponse = await oauthClientCredentialClient.GetClientCredentialsAsync();

                token = tokenResponse.AccessToken;
            }

            return token;
        }
    }
}