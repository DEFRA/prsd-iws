namespace EA.Iws.RequestHandlers.Admin
{
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class GetNotificationInfoHandler : IRequestHandler<GetNotificationInfo, NotificationInfo>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;

        public GetNotificationInfoHandler(INotificationApplicationRepository notificationApplicationRepository,
            INotificationAssessmentRepository notificationAssessmentRepository,
            IImportNotificationRepository importNotificationRepository,
            IImportNotificationAssessmentRepository importNotificationAssessmentRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.importNotificationRepository = importNotificationRepository;
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
        }

        public async Task<NotificationInfo> HandleAsync(GetNotificationInfo message)
        {
            var id = await notificationApplicationRepository.GetIdOrDefault(message.Number);
            var importId = await importNotificationRepository.GetIdOrDefault(message.Number);

            if (!id.HasValue && !importId.HasValue)
            {
                return new NotificationInfo
                {
                    IsExistingNotification = false
                };
            }

            if (id.HasValue)
            {
                var notificationAssessment = await notificationAssessmentRepository.GetByNotificationId(id.Value);

                return new NotificationInfo
                {
                    IsExistingNotification = true,
                    Id = id.Value,
                    ExportNotificationStatus = notificationAssessment.Status,
                    TradeDirection = TradeDirection.Export
                };
            }
            else
            {
                var notificationAssessment = await importNotificationAssessmentRepository.GetByNotification(importId.Value);

                return new NotificationInfo
                {
                    IsExistingNotification = true,
                    Id = importId.Value,
                    ImportNotificationStatus = notificationAssessment.Status,
                    TradeDirection = TradeDirection.Import
                };
            }
        }
    }
}
