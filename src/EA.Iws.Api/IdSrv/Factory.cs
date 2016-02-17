namespace EA.Iws.Api.IdSrv
{
    using IdentityServer3.Core.Configuration;
    using IdentityServer3.Core.Services;
    using IdentityServer3.Core.Services.InMemory;
    using IdentityServer3.EntityFramework;
    using Services;

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