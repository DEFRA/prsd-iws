namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
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
                NotificationReceived = DateTimeOffsetAsDateTime(assessment.Dates.NotificationReceivedDate),
                PaymentReceived = DateTimeOffsetAsDateTime(assessment.Dates.PaymentReceivedDate),
                IsPaymentComplete = assessment.Dates.PaymentReceivedDate.HasValue &&
                await transactionCalculator.PaymentIsNowFullyReceived(message.ImportNotificationId, 0),
                NameOfOfficer = assessment.Dates.NameOfOfficer,
                AssessmentStarted = DateTimeOffsetAsDateTime(assessment.Dates.AssessmentStartedDate),
                NotificationCompletedDate = DateTimeOffsetAsDateTime(assessment.Dates.NotificationCompletedDate),
                AcknowlegedDate = DateTimeOffsetAsDateTime(assessment.Dates.AcknowledgedDate),
                DecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(assessment),
                IsInterim = interimStatus.IsInterim
            };
        }

        private DateTime? DateTimeOffsetAsDateTime(DateTimeOffset? dateTimeOffset)
        {
            if (dateTimeOffset.HasValue)
            {
                return dateTimeOffset.Value.DateTime;
            }

            return null;
        }
    }
}
