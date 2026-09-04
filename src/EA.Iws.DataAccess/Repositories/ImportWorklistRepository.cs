namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Core.Notification;
    using Domain.ImportNotificationAssessment;

    internal class ImportWorklistRepository : IImportWorklistRepository
    {
        private readonly IwsContext context;

        public ImportWorklistRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ImportWorklistQueryResult> GetByCompetentAuthority(
            UKCompetentAuthority competentAuthority,
            string notificationNumber,
            string officer,
            ImportNotificationStatus[] statuses,
            int pageNumber,
            int pageSize)
        {
            var statusParam = statuses != null && statuses.Length > 0
                ? string.Join(",", statuses.Select(s => (int)s))
                : null;

            var parameters = new[]
            {
                new SqlParameter("@CompetentAuthority", (int)competentAuthority),
                new SqlParameter("@NotificationNumber", (object)notificationNumber ?? DBNull.Value),
                new SqlParameter("@Officer", (object)officer ?? DBNull.Value),
                new SqlParameter("@Statuses", (object)statusParam ?? DBNull.Value),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize)
            };

            var results = await context.Database.SqlQuery<ImportWorklistRow>(
                @"EXEC [ImportNotification].[uspGetImportWorklist] 
                    @CompetentAuthority, 
                    @NotificationNumber, 
                    @Officer, 
                    @Statuses, 
                    @PageNumber, 
                    @PageSize",
                parameters).ToListAsync();

            var totalCount = results.Any() ? results.First().TotalCount : 0;

            return new ImportWorklistQueryResult
            {
                TotalCount = totalCount,
                PagedRows = results.Select(x =>
                    ImportWorklistSummary.Load(
                        x.NotificationId,
                        x.NotificationNumber,
                        x.Exporter,
                        x.Officer,
                        x.DatePickedUpByOfficer,
                        x.NotificationReceivedDate,
                        x.AcknowledgedDate,
                        x.ConsentedDate,
                        x.DecisionRequiredByDate,
                        x.Status,
                        x.LastCommentDate,
                        x.FinancialGuaranteeStatus,
                        x.FinancialGuaranteeStatusDescription,
                        x.LastAction,
                        x.LastCommentUser)).ToArray()
            };
        }
    }
}