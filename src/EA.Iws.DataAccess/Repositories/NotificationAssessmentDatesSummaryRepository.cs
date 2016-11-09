namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.Security;

    public class NotificationAssessmentDatesSummaryRepository : INotificationAssessmentDatesSummaryRepository
    {
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly INotificationApplicationAuthorization authorization;

        public NotificationAssessmentDatesSummaryRepository(DecisionRequiredBy decisionRequiredBy,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationTransactionCalculator transactionCalculator,
            INotificationApplicationAuthorization authorization)
        {
            this.decisionRequiredBy = decisionRequiredBy;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.transactionCalculator = transactionCalculator;
            this.authorization = authorization;
        }

        public async Task<NotificationDatesSummary> GetById(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            var assessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);
            var notification = await notificationApplicationRepository.GetById(notificationId);

            var latestPayment = await transactionCalculator.LatestPayment(notificationId);
            DateTime? lastestPaymentDate = null;

            if (latestPayment != null)
            {
                lastestPaymentDate = latestPayment.Date;
            }

            return NotificationDatesSummary.Load(
                assessment.Status,
                assessment.Dates.NotificationReceivedDate,
                notificationId,
                lastestPaymentDate,
                await transactionCalculator.IsPaymentComplete(notificationId),
                assessment.Dates.CommencementDate,
                assessment.Dates.NameOfOfficer,
                assessment.Dates.CompleteDate,
                assessment.Dates.TransmittedDate,
                assessment.Dates.AcknowledgedDate,
                await decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment),
                assessment.Dates.FileClosedDate,
                assessment.Dates.ArchiveReference);
        }
    }
}
