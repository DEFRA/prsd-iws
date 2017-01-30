namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.Reports;

    internal class ImportNotificationsRepository : IImportNotificationsRepository
    {
        private readonly IwsContext context;

        public ImportNotificationsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<DataImportNotification>> Get(DateTime @from, DateTime to,
            UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<DataImportNotification>(
                @"SELECT	[NotificationNumber],
                            [NotificationType],
                            [Status],
                            [Preconsented],
                            [NotificationReceived],
                            [PaymentReceived],
                            [AssessmentStarted],
                            [ApplicationCompleted],
                            [Acknowledged],
                            [DecisionDate],
                            [Consented],
                            [Officer]

                    FROM	[Reports].[DataImportNotifications]

                    WHERE	[CompetentAuthorityId] = @ca
                    AND     [NotificationReceived] BETWEEN @from AND @to",
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to)).ToArrayAsync();
        }
    }
}