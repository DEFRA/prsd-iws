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
    using ImportNotification = Domain.ImportNotification.ImportNotification;

    internal class CreateImportNotificationHandler : IRequestHandler<CreateImportNotification, Guid>
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IInterimStatusRepository interimStatusRepository;
        private readonly IUserContext userContext;

        public CreateImportNotificationHandler(IImportNotificationRepository importNotificationRepository,
            ImportNotificationContext context, 
            IUserContext userContext, 
            IInternalUserRepository internalUserRepository,
            IImportNotificationAssessmentRepository assessmentRepository,
            IInterimStatusRepository interimStatusRepository)
        {
            this.internalUserRepository = internalUserRepository;
            this.assessmentRepository = assessmentRepository;
            this.interimStatusRepository = interimStatusRepository;
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

            var assessment = await assessmentRepository.GetByNotification(notification.Id);

            assessment.Receive(message.ReceivedDate);

            interimStatusRepository.Add(new InterimStatus(notification.Id, message.IsInterim));

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}