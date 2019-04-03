namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class ExportNotificationsRepository : IExportNotificationsRepository
    {
        private readonly IwsContext context;

        public ExportNotificationsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<DataExportNotification>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<DataExportNotification>(
                @"SELECT	[NotificationNumber],
                            [NotificationType],
                            [Preconsented],
                            [Status],
                            [NotificationReceived],
                            [PaymentReceived],
                            [AssessmentStarted],
                            [ApplicationCompleted],
                            [Transmitted],
                            [Acknowledged],
                            [DecisionDate],
                            [Consented],
                            [Officer],
                            [SubmittedBy],
                            [ConsentTo]
                    FROM	[Reports].[DataExportNotifications]

                    WHERE	[CompetentAuthorityId] = @ca
                    AND     [NotificationReceived] BETWEEN @from AND @to",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToArrayAsync();
        }
    }
}
