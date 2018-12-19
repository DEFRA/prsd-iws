namespace EA.Iws.RequestHandlers.SharedUsers
{
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;
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
            var totalCount = await repository.GetSharedUserCount(message.NotificationId);

            if (totalCount > 0)
            {
                return true;
            }
            return false;
        }
    }
}
