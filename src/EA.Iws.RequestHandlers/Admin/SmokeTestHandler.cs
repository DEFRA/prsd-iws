namespace EA.Iws.RequestHandlers.Admin
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class SmokeTestHandler : IRequestHandler<SmokeTest, bool>
    {
        private readonly IwsContext context;

        public SmokeTestHandler(IwsContext context)
        {
            this.context = context;
        }

        public Task<bool> HandleAsync(SmokeTest message)
        {
            return Task.FromResult(context.Database.Exists());
        }
    }
}