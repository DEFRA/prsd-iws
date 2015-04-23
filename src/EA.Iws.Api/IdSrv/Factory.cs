namespace EA.Iws.Api.IdSrv
{
    using Thinktecture.IdentityServer.Core.Configuration;
    using Thinktecture.IdentityServer.Core.Services;
    using Thinktecture.IdentityServer.Core.Services.InMemory;

    internal static class Factory
    {
        public static IdentityServerServiceFactory Configure()
        {
            var factory = new IdentityServerServiceFactory();

            var scopeStore = new InMemoryScopeStore(Scopes.Get());
            factory.ScopeStore = new Registration<IScopeStore>(scopeStore);
            var clientStore = new InMemoryClientStore(Clients.Get());
            factory.ClientStore = new Registration<IClientStore>(clientStore);
            var users = new InMemoryUserService(Users.Get());
            factory.UserService = new Registration<IUserService>(users);

            return factory;
        }
    }
}