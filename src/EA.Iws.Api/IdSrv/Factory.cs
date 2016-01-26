namespace EA.Iws.Api.IdSrv
{
    using Services;
    using Thinktecture.IdentityServer.Core.Configuration;
    using Thinktecture.IdentityServer.Core.Services;
    using Thinktecture.IdentityServer.Core.Services.InMemory;
    using Thinktecture.IdentityServer.EntityFramework;

    internal static class Factory
    {
        public static IdentityServerServiceFactory Configure(AppConfiguration config)
        {
            var factory = new IdentityServerServiceFactory();

            var scopeStore = new InMemoryScopeStore(Scopes.Get());
            factory.ScopeStore = new Registration<IScopeStore>(scopeStore);
            var clientStore = new InMemoryClientStore(Clients.Get(config));
            factory.ClientStore = new Registration<IClientStore>(clientStore);

            var efConfig = new EntityFrameworkServiceOptions
            {
                ConnectionString = "Iws.DefaultConnection",
                Schema = "Identity"
            };

            factory.RegisterOperationalServices(efConfig);

            var cleanup = new TokenCleanup(efConfig);
            cleanup.Start();

            return factory;
        }
    }
}