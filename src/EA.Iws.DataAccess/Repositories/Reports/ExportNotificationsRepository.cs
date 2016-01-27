namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Domain;
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
		                    [Status],
		                    [NotificationReceived],
		                    [PaymentReceived],
		                    [AssessmentStarted],
		                    [ApplicationCompleted],
		                    [Transmitted],
		                    [Acknowledged],
		                    [DecisionDate],
                            [Consented],
                            [Officer]

                    FROM	[Reports].[DataExportNotifications]

                    WHERE	[CompetentAuthorityId] = @ca",
                new SqlParameter("@ca", competentAuthority.Value)).ToArrayAsync();
        }
    }
}
