namespace EA.Iws.RequestHandlers.SharedUsers
{
    using System.Linq;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.SharedUsers;
    using System.Threading.Tasks;

    internal class CheckIfSharedUserExistsHandler : IRequestHandler<CheckIfSharedUserExists, bool>
    {
        private readonly ISharedUserRepository repository;

        public CheckIfSharedUserExistsHandler(ISharedUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> HandleAsync(CheckIfSharedUserExists message)
        {
            var sharedUsers = await repository.GetAllSharedUsers(message.NotificationId);

            return sharedUsers.Any();
        }
    }
}
