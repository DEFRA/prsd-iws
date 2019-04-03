namespace EA.Iws.RequestHandlers.Users
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Users;

    internal class GetUserIsInternalHandler : IRequestHandler<GetUserIsInternal, bool>
    {
        private readonly IwsContext context;

        public GetUserIsInternalHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(GetUserIsInternal message)
        {
            return await context.IsInternal();
        }
    }
}
