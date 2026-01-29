namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationInterimStatusHandler : IRequestHandler<GetImportInterimStatus, ImportInterimStatus>
    {
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IInterimStatusRepository interimStatusRepository;

        public GetImportNotificationInterimStatusHandler(
            IImportNotificationAssessmentRepository importNotificationAssessmentRepository,
            IInterimStatusRepository interimStatusRepository)
        {
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
            this.interimStatusRepository = interimStatusRepository;
        }

        public async Task<ImportInterimStatus> HandleAsync(GetImportInterimStatus message)
        {
            var assessment = await importNotificationAssessmentRepository.GetByNotification(message.NotificationId);
            var interimStatus = await interimStatusRepository.GetByNotificationId(message.NotificationId);

            return new ImportInterimStatus
            {
                IsInterim = interimStatus?.IsInterim,
                NotificationId = assessment.NotificationApplicationId,
                NotificationStatus = assessment.Status
            };
        }
    }
}