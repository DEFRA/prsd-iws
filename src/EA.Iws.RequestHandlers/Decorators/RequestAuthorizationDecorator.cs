namespace EA.Iws.RequestHandlers.Decorators
{
    using System.Security;
    using System.Threading.Tasks;
    using Authorization;
    using Core.Authorization;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;

    internal class RequestAuthorizationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;
        private readonly IUserContext userContext;
        private readonly IAuthorizationManager authorizationManager;
        private readonly RequestAuthorizationAttributeCache requestAuthorizationAttributeCache;

        public RequestAuthorizationDecorator(IRequestHandler<TRequest, TResponse> inner,
            IUserContext userContext,
            IAuthorizationManager authorizationManager,
            RequestAuthorizationAttributeCache requestAuthorizationAttributeCache)
        {
            this.inner = inner;
            this.userContext = userContext;
            this.authorizationManager = authorizationManager;
            this.requestAuthorizationAttributeCache = requestAuthorizationAttributeCache;
        }

        public async Task<TResponse> HandleAsync(TRequest message)
        {
            if (await HasAccess(message))
            {
                return await inner.HandleAsync(message).ConfigureAwait(true);
            }

            throw RequestAuthorizationException.CreateForRequest(message);
        }

        private async Task<bool> HasAccess(TRequest message)
        {
            var requestAuthorizationAttribute = requestAuthorizationAttributeCache.Get(message.GetType());

            if (requestAuthorizationAttribute == null)
            {
                return true;
            }

            var context = new AuthorizationContext(userContext.Principal, 
                requestAuthorizationAttribute.Name);

            return await authorizationManager.CheckAccessAsync(context).ConfigureAwait(true);
        }
    }
}
