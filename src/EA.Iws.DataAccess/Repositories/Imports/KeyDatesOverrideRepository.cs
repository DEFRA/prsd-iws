namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.ImportNotificationAssessment;

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
                    D.[AssessmentStartedDate] AS [CommencementDate],
                    D.[NotificationCompletedDate] AS [CompleteDate],
                    NULL AS [TransmittedDate],
                    D.[AcknowledgedDate],
                    D.[WithdrawnDate],
                    O.[Date] AS [ObjectedDate],
                    D.[ConsentedDate],
                    C.[From] AS [ConsentValidFromDate],
                    C.[To] AS [ConsentValidToDate]
                FROM
                    [ImportNotification].[NotificationAssessment] NA
                    INNER JOIN [ImportNotification].[NotificationDates] D ON NA.Id = D.NotificationAssessmentId
                    LEFT JOIN [ImportNotification].[Consent] C ON NA.NotificationApplicationId = C.NotificationId
                    LEFT JOIN [ImportNotification].[Objection] O ON NA.NotificationApplicationId = O.NotificationId
                WHERE
                    NA.NotificationApplicationId = @NotificationId",
                new SqlParameter("@NotificationId", notificationId)).SingleAsync();
        }

        public async Task SetKeyDatesForNotification(KeyDatesOverrideData data)
        {
            await context.Database.ExecuteSqlCommandAsync(@"[ImportNotification].[uspUpdateImportNotificationKeyDates] 
                @NotificationId
                ,@NotificationReceivedDate
                ,@AssessmentStartedDate
                ,@CompleteDate
                ,@AcknowledgedDate
                ,@WithdrawnDate
                ,@ObjectedDate
                ,@ConsentedDate
                ,@ConsentValidFromDate
                ,@ConsentValidToDate",
                new SqlParameter("@NotificationId", data.NotificationId),
                new SqlParameter("@NotificationReceivedDate", (object)data.NotificationReceivedDate ?? DBNull.Value),
                new SqlParameter("@AssessmentStartedDate", (object)data.CommencementDate ?? DBNull.Value),
                new SqlParameter("@CompleteDate", (object)data.CompleteDate ?? DBNull.Value),
                new SqlParameter("@AcknowledgedDate", (object)data.AcknowledgedDate ?? DBNull.Value),
                new SqlParameter("@WithdrawnDate", (object)data.WithdrawnDate ?? DBNull.Value),
                new SqlParameter("@ObjectedDate", (object)data.ObjectedDate ?? DBNull.Value),
                new SqlParameter("@ConsentedDate", (object)data.ConsentedDate ?? DBNull.Value),
                new SqlParameter("@ConsentValidFromDate", (object)data.ConsentValidFromDate ?? DBNull.Value),
                new SqlParameter("@ConsentValidToDate", (object)data.ConsentValidToDate ?? DBNull.Value));
        }
    }
}