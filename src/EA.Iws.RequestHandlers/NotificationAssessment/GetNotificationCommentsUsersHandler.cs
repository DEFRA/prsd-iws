namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Prsd.Core.Mediator;

    internal class GetNotificationCommentsUsersHandler : IRequestHandler<GetNotificationCommentsUsers, NotificationCommentsUsersData>
    {
        private readonly INotificationCommentRepository repository;
        private readonly IInternalUserRepository internalUserRepository;

        public GetNotificationCommentsUsersHandler(INotificationCommentRepository repository, IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<NotificationCommentsUsersData> HandleAsync(GetNotificationCommentsUsers message)
        {
            var users = await this.repository.GetUsers(message.NotificationId, message.Type);

            NotificationCommentsUsersData returnData = new NotificationCommentsUsersData();
            foreach (string userId in users)
            {
                var user = Task.Run(() => internalUserRepository.GetByUserId(userId)).Result.User.FullName;

                returnData.Users.Add(userId, user);
            }

            return returnData;
        }
    }
}
