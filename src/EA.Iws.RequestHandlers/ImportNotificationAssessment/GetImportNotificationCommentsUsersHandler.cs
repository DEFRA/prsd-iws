namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Domain;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationCommentsUsersHandler : IRequestHandler<GetImportNotificationCommentsUsers, ImportNotificationCommentsUsersData>
    {
        private readonly IImportNotificationCommentRepository repository;
        private readonly IInternalUserRepository internalUserRepository;

        public GetImportNotificationCommentsUsersHandler(IImportNotificationCommentRepository repository, IInternalUserRepository internalUserRepository)
        {
            this.repository = repository;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<ImportNotificationCommentsUsersData> HandleAsync(GetImportNotificationCommentsUsers message)
        {
            var users = await this.repository.GetUsers(message.NotificationId, message.Type);

            ImportNotificationCommentsUsersData returnData = new ImportNotificationCommentsUsersData();
            foreach (string userId in users)
            {
                var user = await this.internalUserRepository.GetByUserId(userId);

                returnData.Users.Add(userId, user.User.FullName);
            }

            return returnData;
        }
    }
}
