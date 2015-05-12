namespace EA.Iws.Cqrs.Users
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;

    internal class UserByIdHandler : IRequestHandler<UserById, User>
    {
        private readonly IwsContext context;

        public UserByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<User> HandleAsync(UserById query)
        {
            return await context.Users.Include(u => u.Organisation).SingleOrDefaultAsync(u => u.Id == query.Id);
        }
    }
}