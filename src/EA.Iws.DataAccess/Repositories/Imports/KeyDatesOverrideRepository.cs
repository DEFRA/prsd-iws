namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Core.Admin.KeyDates;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Domain.ImportNotificationAssessment.Consent;
    using EA.Iws.Domain.ImportNotificationAssessment.Decision;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class KeyDatesOverrideRepository : IKeyDatesOverrideRepository
    {
        private readonly IwsContext context;
        private readonly ImportNotificationContext importContext;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IImportConsentRepository importConsentRepository;
        private readonly IImportWithdrawnRepository importWithdrawnRepository;
        private readonly IImportObjectionRepository importObjectionRepository;

        public KeyDatesOverrideRepository(IwsContext context,
            ImportNotificationContext importContext,
            IImportNotificationAssessmentRepository importNotificationAssessmentRepository,
            IImportConsentRepository importConsentRepository,
            IImportWithdrawnRepository importWithdrawnRepository,
            IImportObjectionRepository importObjectionRepository)
        {
            this.context = context;
            this.importContext = importContext;
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
            this.importConsentRepository = importConsentRepository;
            this.importWithdrawnRepository = importWithdrawnRepository;
            this.importObjectionRepository = importObjectionRepository;
        }

        public async Task<KeyDatesOverrideData> GetKeyDatesForNotification(Guid notificationId)
        {
            return await context.Database.SqlQuery<KeyDatesOverrideData>(@"
                SELECT
                    NA.[NotificationApplicationId] AS [NotificationId],
                    D.[NotificationReceivedDate],
                    D.[NotificationChargeDate],
                    D.[AssessmentStartedDate] AS [CommencementDate],
                    D.[NotificationCompletedDate] AS [CompleteDate],
                    NULL AS [TransmittedDate],
                    D.[AcknowledgedDate],
                    W.[Date] AS [WithdrawnDate],
                    O.[Date] AS [ObjectedDate],
                    D.[ConsentedDate],
                    C.[From] AS [ConsentValidFromDate],
                    C.[To] AS [ConsentValidToDate]
                FROM
                    [ImportNotification].[NotificationAssessment] NA
                    INNER JOIN [ImportNotification].[NotificationDates] D ON NA.Id = D.NotificationAssessmentId
                    LEFT JOIN [ImportNotification].[Consent] C ON NA.NotificationApplicationId = C.NotificationId
                    LEFT JOIN [ImportNotification].[Objection] O ON NA.NotificationApplicationId = O.NotificationId
                    LEFT JOIN [ImportNotification].[Withdrawn] W ON NA.NotificationApplicationId = W.NotificationId
                WHERE
                    NA.NotificationApplicationId = @NotificationId",
                new SqlParameter("@NotificationId", notificationId)).SingleAsync();
        }

        public async Task SetKeyDatesForNotification(KeyDatesOverrideData data)
        {
            var assessment = await importNotificationAssessmentRepository.GetByNotification(data.NotificationId);
            assessment.UpdateKeyDates(data);

            var consent = await importConsentRepository.GetByNotificationIdOrDefault(data.NotificationId);
            if (data.ConsentValidFromDate != null && data.ConsentValidToDate != null)
            {
                consent?.UpdateDateRange(data.ConsentValidFromDate.Value, data.ConsentValidToDate.Value);
            }

            var withdrawn = await importWithdrawnRepository.GetByNotificationIdOrDefault(data.NotificationId);
            withdrawn?.UpdateDate(data.WithdrawnDate);

            var objection = await importObjectionRepository.GetByNotificationIdOrDefault(data.NotificationId);
            objection?.UpdateDate(data.ObjectedDate);

            await importContext.SaveChangesAsync();
        }

        public async Task SetDecisionRequiredByDateForNotification(Guid notificationAssessmentId, DateTime? decisionRequiredByDate)
        {
            await context.Database.ExecuteSqlCommandAsync(@"
                UPDATE [ImportNotification].[NotificationDates] SET [DecisionRequiredByDate] = @DecisionRequiredByDate WHERE [NotificationAssessmentId] = @NotificationAssessmentId",
                new SqlParameter("@NotificationAssessmentId", notificationAssessmentId),
                new SqlParameter("@DecisionRequiredByDate", decisionRequiredByDate));
        }
    }
}