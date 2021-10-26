namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Prsd.Core.Domain;

    public class ImportNotificationSearchRepository : IImportNotificationSearchRepository
    {
        private readonly IwsContext context;
        private readonly ImportNotificationContext importNotificationContext;
        private readonly IUserContext userContext;

        public ImportNotificationSearchRepository(IwsContext context, ImportNotificationContext importNotificationContext, IUserContext userContext)
        {
            this.context = context;
            this.importNotificationContext = importNotificationContext;
            this.userContext = userContext;
        }

        public async Task<IEnumerable<ImportNotificationSearchResult>> SearchBySearchTerm(string searchTerm)
        {
            var userCompetentAuthority = await context.InternalUsers
                .Where(u => u.UserId == userContext.UserId.ToString())
                .Select(u => u.CompetentAuthority)
                .SingleAsync();

            var query = from notification
                in importNotificationContext.ImportNotifications
                join importer in importNotificationContext.Importers on notification.Id equals importer.ImportNotificationId
                where (notification.CompetentAuthority == userCompetentAuthority && 
                       (notification.NotificationNumber.ToLower().Replace(" ", string.Empty).Contains(searchTerm.ToLower().Replace(" ", string.Empty)) || 
                        importer.Name.ToLower().Contains(searchTerm.ToLower())))
                from assessment
                in importNotificationContext.ImportNotificationAssessments
                    .Where(a => a.NotificationApplicationId == notification.Id)
                from exporter
                    in importNotificationContext.Exporters
                    .Where(e => e.ImportNotificationId == notification.Id)
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
                x.Assessment != null && (x.Assessment.Status == ImportNotificationStatus.Consented || x.Assessment.Status == ImportNotificationStatus.ConsentWithdrawn)));
        }
    }
}
