namespace EA.Iws.DataAccess.Repositories
{
    using Core.Admin.KeyDates;
    using Domain.NotificationAssessment;
    using EA.Iws.Domain.NotificationConsent;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class KeyDatesOverrideRepository : IKeyDatesOverrideRepository
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationConsentRepository notificationConsentRepository;

        public KeyDatesOverrideRepository(IwsContext context, INotificationAssessmentRepository notificationAssessmentRepository, INotificationConsentRepository notificationConsentRepository)
        {
            this.context = context;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.notificationConsentRepository = notificationConsentRepository;
        }

        public async Task<KeyDatesOverrideData> GetKeyDatesForNotification(Guid notificationId)
        {
            return await context.Database.SqlQuery<KeyDatesOverrideData>(@"
                SELECT
                    NA.[NotificationApplicationId] AS [NotificationId],
                    D.[NotificationReceivedDate],
                    D.[CommencementDate],
                    D.[CompleteDate],
                    D.[TransmittedDate],
                    D.[AcknowledgedDate],
                    D.[WithdrawnDate],
                    D.[ObjectedDate],
                    D.[ConsentedDate],
                    D.[NotificationChargeDate],
                    C.[From] AS [ConsentValidFromDate],
                    C.[To] AS [ConsentValidToDate]
                FROM
                    [Notification].[NotificationAssessment] NA
                    INNER JOIN [Notification].[NotificationDates] D ON NA.Id = D.NotificationAssessmentId
                    LEFT JOIN [Notification].[Consent] C ON NA.NotificationApplicationId = C.NotificationApplicationId
                WHERE
                    NA.NotificationApplicationId = @NotificationId",
                new SqlParameter("@NotificationId", notificationId)).SingleAsync();
        }

        public async Task SetKeyDatesForNotification(KeyDatesOverrideData data)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(data.NotificationId);
            assessment.UpdateKeyDates(data);

            var consent = await notificationConsentRepository.GetByNotificationId(data.NotificationId);

            if (data.ConsentValidFromDate != null && data.ConsentValidToDate != null)
            {
                consent.ConsentRange = new Domain.DateRange(data.ConsentValidFromDate.Value, data.ConsentValidToDate.Value);
            }

            await context.SaveChangesAsync();
        }

        public async Task SetDecisionRequiredByDateForNotification(Guid notificationAssessmentId, DateTime? decisionRequiredByDate)
        {
            await context.Database.ExecuteSqlCommandAsync(@"
                UPDATE [Notification].[NotificationDates] SET [DecisionRequiredByDate] = @DecisionRequiredByDate WHERE [NotificationAssessmentId] = @NotificationAssessmentId",
                new SqlParameter("@NotificationAssessmentId", notificationAssessmentId),
                new SqlParameter("@DecisionRequiredByDate", decisionRequiredByDate));
        }
    }
}