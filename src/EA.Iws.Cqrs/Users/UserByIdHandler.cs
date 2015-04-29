namespace EA.Iws.Cqrs.Users
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    internal class UserByIdHandler : IQueryHandler<UserById, User>
    {
        private readonly IwsContext context;

        public UserByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<User> ExecuteAsync(UserById query)
        {
            return await context.Users.Include(u => u.Organisation).SingleOrDefaultAsync(u => u.Id == query.Id);
        }
    }
}
