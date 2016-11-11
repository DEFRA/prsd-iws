namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
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

        public GetKeyDatesHandler(IImportNotificationAssessmentRepository notificationAssessmentRepository,
            IInterimStatusRepository interimStatusRepository,
            DecisionRequiredBy decisionRequiredBy,
            IImportNotificationTransactionCalculator transactionCalculator)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.interimStatusRepository = interimStatusRepository;
            this.decisionRequiredBy = decisionRequiredBy;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<KeyDatesData> HandleAsync(GetKeyDates message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotification(message.ImportNotificationId);
            var interimStatus = await interimStatusRepository.GetByNotificationId(message.ImportNotificationId);

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
                ArchiveReference = assessment.Dates.ArchiveReference
            };
        }
    }
}
