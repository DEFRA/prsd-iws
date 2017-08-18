namespace EA.Iws.RequestHandlers.Users
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Users;

    internal class ExternalUserExistsHandler : IRequestHandler<ExternalUserExists, bool>
    {
        private readonly IwsContext context;

        public ExternalUserExistsHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(ExternalUserExists message)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == message.EmailAddress);

            if (user == null)
            {
                return false;
            }

            var isInternal = await context.InternalUsers.AnyAsync(u => u.UserId == user.Id);

            return !isInternal;
        }
    }
}