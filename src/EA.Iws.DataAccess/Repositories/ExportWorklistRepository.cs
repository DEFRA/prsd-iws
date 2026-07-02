namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
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
            var query =
                context.NotificationAssessments
                    .Join(context.NotificationApplications,
                        assessment => assessment.NotificationApplicationId,
                        notification => notification.Id,
                        (assessment, notification) => new { Assessment = assessment, Notification = notification })
                    .Where(x => x.Notification.CompetentAuthority == competentAuthority)
                    .GroupJoin(context.Exporters,
                        x => x.Notification.Id,
                        exporter => exporter.NotificationId,
                        (x, exporters) => new { x.Assessment, x.Notification, Exporters = exporters })
                    .SelectMany(
                        x => x.Exporters.DefaultIfEmpty(),
                        (x, exporter) => new { x.Assessment, x.Notification, Exporter = exporter });

            if (!string.IsNullOrWhiteSpace(notificationNumber))
            {
                query = query.Where(x => x.Notification.NotificationNumber.Contains(notificationNumber));
            }

            if (!string.IsNullOrWhiteSpace(officer))
            {
                query = query.Where(x => x.Assessment.Dates.NameOfOfficer.Contains(officer));
            }

            if (statuses != null && statuses.Length > 0)
            {
                query = query.Where(x => statuses.Contains(x.Assessment.Status));
            }

            var totalCount = await query.CountAsync();

            var rows = await query
                .OrderBy(x => x.Assessment.Dates.DecisionRequiredByDate)
                .ThenBy(x => x.Notification.NotificationNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    x.Notification.Id,
                    x.Notification.NotificationNumber,
                    NotifierName = x.Exporter != null ? x.Exporter.Business.Name : null,
                    x.Assessment.Dates.NameOfOfficer,
                    x.Assessment.Dates.CommencementDate,
                    x.Assessment.Dates.TransmittedDate,
                    x.Assessment.Dates.AcknowledgedDate,
                    x.Assessment.Dates.ConsentedDate,
                    x.Assessment.Dates.DecisionRequiredByDate,
                    x.Assessment.Status,
                    LastAuditDate = context.NotificationAudit
                        .Where(a => a.NotificationId == x.Notification.Id)
                        .OrderByDescending(a => a.DateAdded)
                        .Select(a => (DateTimeOffset?)a.DateAdded)
                        .FirstOrDefault(),
                    LastAuditType = context.NotificationAudit
                        .Where(a => a.NotificationId == x.Notification.Id)
                        .OrderByDescending(a => a.DateAdded)
                        .Select(a => a.Type.ToString())
                        .FirstOrDefault(),
                    LastCommentDate = context.NotificationComments
                        .Where(c => c.NotificationId == x.Notification.Id)
                        .OrderByDescending(c => c.DateAdded)
                        .Select(c => (DateTimeOffset?)c.DateAdded)
                        .FirstOrDefault()
                }).ToArrayAsync();

            return new ExportWorklistQueryResult
            {
                TotalCount = totalCount,
                PagedRows = rows.Select(x =>
                    ExportWorklistSummary.Load(
                        x.Id,
                        x.NotificationNumber,
                        x.NotifierName,
                        x.NameOfOfficer,
                        x.CommencementDate,
                        x.TransmittedDate,
                        x.AcknowledgedDate,
                        x.ConsentedDate,
                        x.DecisionRequiredByDate,
                        x.Status,
                        x.LastAuditDate,
                        x.LastAuditType,
                        x.LastCommentDate)).ToArray()
            };
        }
    }
}