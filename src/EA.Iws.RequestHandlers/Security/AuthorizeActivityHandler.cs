namespace EA.Iws.RequestHandlers.Security
{
    using System.Threading.Tasks;
    using Authorization;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Security;

    internal class AuthorizeActivityHandler : IRequestHandler<AuthorizeActivity, bool>
    {
        private readonly IUserContext userContext;
        private readonly IAuthorizationManager authorizationManager;

        public AuthorizeActivityHandler(IAuthorizationManager authorizationManager,
            IUserContext userContext)
        {
            this.authorizationManager = authorizationManager;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(AuthorizeActivity message)
        {
            var authorizationContext = new AuthorizationContext(userContext.Principal, message.Activity);

            return await authorizationManager.CheckAccessAsync(authorizationContext);
        }
    }
}