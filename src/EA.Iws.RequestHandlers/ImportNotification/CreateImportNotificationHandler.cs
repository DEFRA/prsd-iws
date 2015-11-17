namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.ImportNotification;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class CreateImportNotificationHandler : IRequestHandler<CreateImportNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public CreateImportNotificationHandler(IImportNotificationRepository importNotificationRepository,
            IwsContext context, IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.internalUserRepository = internalUserRepository;
            this.importNotificationRepository = importNotificationRepository;
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(CreateImportNotification message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            var notification = new ImportNotification(message.NotificationType, user.CompetentAuthority, message.Number);

            await importNotificationRepository.Add(notification);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}