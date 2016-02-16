namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    public class ImportNotificationSearchRepository : IImportNotificationSearchRepository
    {
        private readonly ImportNotificationContext context;

        public ImportNotificationSearchRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ImportNotificationSearchResult>> SearchByNumber(string number)
        {
            var query = from notification
                in context.ImportNotifications
                where notification.NotificationNumber.Contains(number)
                from assessment 
                    in context.ImportNotificationAssessments
                        .Where(a => a.NotificationApplicationId == notification.Id)
                from exporter
                    in context.Exporters
                    .Where(e => e.ImportNotificationId == notification.Id)
                .DefaultIfEmpty()
                from importer
                    in context.Importers
                    .Where(i => i.ImportNotificationId == notification.Id)
                    .DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    Assessment = assessment,
                    Importer = importer,
                    Exporter = exporter
                };

            var data = await query
                .OrderBy(x => x.Notification.NotificationNumber)
                .ToListAsync();

            return data.Select(x => new ImportNotificationSearchResult(x.Notification.Id,
                x.Notification.NotificationNumber,
                x.Assessment.Status,
                x.Exporter == null ? string.Empty : x.Exporter.Name,
                x.Importer == null ? string.Empty : x.Importer.Name,
                x.Notification.NotificationType,
                x.Assessment.Dates.PaymentReceivedDate.HasValue));
        }
    }
}
