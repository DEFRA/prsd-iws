namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;

    internal class ExportWorklistRepository : IExportWorklistRepository
    {
        private readonly IwsContext context;

        public ExportWorklistRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ExportWorklistQueryResult> GetByCompetentAuthority(
            UKCompetentAuthority competentAuthority,
            string notificationNumber,
            string officer,
            NotificationStatus[] statuses,
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

            var results = await context.Database.SqlQuery<ExportWorklistRow>(
                @"EXEC [Notification].[uspGetExportWorklist] 
                    @CompetentAuthority, 
                    @NotificationNumber, 
                    @Officer, 
                    @Statuses, 
                    @PageNumber, 
                    @PageSize",
                parameters).ToListAsync();

            var totalCount = results.Any() ? results.First().TotalCount : 0;

            return new ExportWorklistQueryResult
            {
                TotalCount = totalCount,
                PagedRows = results.Select(x =>
                    ExportWorklistSummary.Load(
                        x.NotificationId,
                        x.NotificationNumber,
                        x.Notifier,
                        x.Officer,
                        x.DatePickedUpByOfficer,
                        x.TransmittedDate,
                        x.AcknowledgedDate,
                        x.ConsentedDate,
                        x.DecisionRequiredByDate,
                        x.Status,
                        x.LastActionDate,
                        x.LastActionType,
                        x.LastCommentDate,
                        x.FinancialGuaranteeStatus,
                        x.LastComment)).ToArray()
            };
        }

        private class ExportWorklistRow
        {
            public Guid NotificationId { get; set; }
            public string NotificationNumber { get; set; }
            public string Notifier { get; set; }
            public string Officer { get; set; }
            public DateTime? DatePickedUpByOfficer { get; set; }
            public DateTime? TransmittedDate { get; set; }
            public DateTime? AcknowledgedDate { get; set; }
            public DateTime? ConsentedDate { get; set; }
            public DateTime? DecisionRequiredByDate { get; set; }
            public NotificationStatus Status { get; set; }
            public DateTimeOffset? LastActionDate { get; set; }
            public string LastActionType { get; set; }
            public DateTimeOffset? LastCommentDate { get; set; }
            public string FinancialGuaranteeStatus { get; set; }
            public string LastComment { get; set; }
            public int TotalCount { get; set; }
        }
    }
}