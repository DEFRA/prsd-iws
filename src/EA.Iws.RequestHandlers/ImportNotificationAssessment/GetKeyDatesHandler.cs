namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    public class GetKeyDatesHandler : IRequestHandler<GetKeyDates, KeyDatesData>
    {
        private readonly IImportNotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IImportNotificationTransactionCalculator transactionCalculator;

        public GetKeyDatesHandler(IImportNotificationAssessmentRepository notificationAssessmentRepository,
            IImportNotificationTransactionCalculator transactionCalculator)
        {
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<KeyDatesData> HandleAsync(GetKeyDates message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotification(message.ImportNotificationId);

            return new KeyDatesData
            {
                NotificationReceived = DateTimeOffsetAsDateTime(assessment.Dates.NotificationReceivedDate),
                PaymentReceived = DateTimeOffsetAsDateTime(assessment.Dates.PaymentReceivedDate),
                IsPaymentComplete = assessment.Dates.PaymentReceivedDate.HasValue &&
                await transactionCalculator.PaymentIsNowFullyReceived(message.ImportNotificationId, 0)
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
