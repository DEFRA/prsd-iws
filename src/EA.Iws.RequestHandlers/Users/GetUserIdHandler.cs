namespace EA.Iws.RequestHandlers.Users
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Users;

    internal class GetUserIdHandler : IRequestHandler<GetUserId, Guid>
    {
        private readonly IwsContext context;

        public GetUserIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(GetUserId message)
        {
            return new Guid((await context.Users.SingleAsync(u => u.Email == message.EmailAddress)).Id);
        }
    }
}