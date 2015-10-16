namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Threading.Tasks;
    using System.Web;
    using Api.Client;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    internal class ApiMediator : IMediator
    {
        private readonly IIwsClient client;
        private readonly HttpContextBase httpContext;

        public ApiMediator(IIwsClient client, HttpContextBase httpContext)
        {
            this.client = client;
            this.httpContext = httpContext;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var allowUnauthorized =
                request.GetType().GetCustomAttributes(typeof(AllowUnauthorizedUserAttribute)).SingleOrDefault();

            if (allowUnauthorized != null)
            {
                return await client.SendAsync(request).ConfigureAwait(false);
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                throw new SecurityException("Unauthenticated user");
            }

            if (!RequestAuthorizationChecker.CheckAccess(request, httpContext))
            {
                throw RequestAuthorizationException.CreateForRequest(request);
            }

            var accessToken = httpContext.User.GetAccessToken();
            return await client.SendAsync(accessToken, request).ConfigureAwait(false);
        }

        public Task<object> SendAsync(object request, Type responseType)
        {
            throw new NotImplementedException();
        }
    }
}