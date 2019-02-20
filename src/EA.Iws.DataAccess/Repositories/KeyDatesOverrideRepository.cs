namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.NotificationAssessment;

    internal class KeyDatesOverrideRepository : IKeyDatesOverrideRepository
    {
        private readonly IwsContext context;

        public KeyDatesOverrideRepository(IwsContext context)
        {
            this.context = context;
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
                    D.[DecisionRequiredByDate],
                    D.[WithdrawnDate],
                    D.[ObjectedDate],
                    D.[ConsentedDate],
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
            await context.Database.ExecuteSqlCommandAsync(@"[Notification].[uspUpdateExportNotificationKeyDates] 
                @NotificationId
                ,@NotificationReceivedDate
                ,@CommencementDate
                ,@CompleteDate
                ,@TransmittedDate
                ,@AcknowledgedDate
                ,@WithdrawnDate
                ,@ObjectedDate
                ,@ConsentedDate
                ,@ConsentValidFromDate
                ,@ConsentValidToDate",
                new SqlParameter("@NotificationId", data.NotificationId),
                new SqlParameter("@NotificationReceivedDate", (object)data.NotificationReceivedDate ?? DBNull.Value),
                new SqlParameter("@CommencementDate", (object)data.CommencementDate ?? DBNull.Value),
                new SqlParameter("@CompleteDate", (object)data.CompleteDate ?? DBNull.Value),
                new SqlParameter("@TransmittedDate", (object)data.TransmittedDate ?? DBNull.Value),
                new SqlParameter("@AcknowledgedDate", (object)data.AcknowledgedDate ?? DBNull.Value),
                new SqlParameter("@WithdrawnDate", (object)data.WithdrawnDate ?? DBNull.Value),
                new SqlParameter("@ObjectedDate", (object)data.ObjectedDate ?? DBNull.Value),
                new SqlParameter("@ConsentedDate", (object)data.ConsentedDate ?? DBNull.Value),
                new SqlParameter("@ConsentValidFromDate", (object)data.ConsentValidFromDate ?? DBNull.Value),
                new SqlParameter("@ConsentValidToDate", (object)data.ConsentValidToDate ?? DBNull.Value));
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