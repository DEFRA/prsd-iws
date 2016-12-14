namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Decision;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    public class GetKeyDatesHandler : IRequestHandler<GetKeyDates, KeyDatesData>
    {
        private readonly IImportNotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IInterimStatusRepository interimStatusRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;
        private readonly IImportNotificationAssessmentDecisionRepository notificationAssessmentDecisionRepository;
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IConsultationRepository consultationRepository;

        public GetKeyDatesHandler(IImportNotificationAssessmentRepository notificationAssessmentRepository,
            IInterimStatusRepository interimStatusRepository,
            DecisionRequiredBy decisionRequiredBy,
            IImportNotificationTransactionCalculator transactionCalculator,
            IImportNotificationAssessmentDecisionRepository notificationAssessmentDecisionRepository,
            IImportNotificationRepository notificationRepository,
            IConsultationRepository consultationRepository)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.interimStatusRepository = interimStatusRepository;
            this.decisionRequiredBy = decisionRequiredBy;
            this.transactionCalculator = transactionCalculator;
            this.notificationAssessmentDecisionRepository = notificationAssessmentDecisionRepository;
            this.notificationRepository = notificationRepository;
            this.consultationRepository = consultationRepository;
        }

        public async Task<KeyDatesData> HandleAsync(GetKeyDates message)
        {
            var notification = await notificationRepository.Get(message.ImportNotificationId);
            var consultation = await consultationRepository.GetByNotificationId(message.ImportNotificationId);
            var assessment = await notificationAssessmentRepository.GetByNotification(message.ImportNotificationId);
            var interimStatus = await interimStatusRepository.GetByNotificationId(message.ImportNotificationId);
            var decisions = await notificationAssessmentDecisionRepository.GetByImportNotificationId(message.ImportNotificationId);

            return new KeyDatesData
            {
                NotificationReceived = assessment.Dates.NotificationReceivedDate,
                PaymentReceived = assessment.Dates.PaymentReceivedDate,
                IsPaymentComplete = assessment.Dates.PaymentReceivedDate.HasValue &&
                await transactionCalculator.PaymentIsNowFullyReceived(message.ImportNotificationId, 0),
                NameOfOfficer = assessment.Dates.NameOfOfficer,
                AssessmentStarted = assessment.Dates.AssessmentStartedDate,
                NotificationCompletedDate = assessment.Dates.NotificationCompletedDate,
                AcknowlegedDate = assessment.Dates.AcknowledgedDate,
                DecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment),
                IsInterim = interimStatus.IsInterim,
                FileClosedDate = assessment.Dates.FileClosedDate,
                ArchiveReference = assessment.Dates.ArchiveReference,
                DecisionHistory = decisions ?? new List<NotificationAssessmentDecision>(),
                CompententAuthority = notification.CompetentAuthority,
                IsLocalAreaSet = consultation != null && consultation.LocalAreaId.HasValue
            };
        }
    }
}
