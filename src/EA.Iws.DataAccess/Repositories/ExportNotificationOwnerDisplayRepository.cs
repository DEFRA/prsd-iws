namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain;
    using Domain.NotificationApplication;

    internal class ExportNotificationOwnerDisplayRepository : IExportNotificationOwnerDisplayRepository
    {
        private readonly IwsContext context;

        public ExportNotificationOwnerDisplayRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExportNotificationOwnerDisplay>> GetInternalUnsubmittedByCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            var query = from Notification in context.NotificationApplications
                            where Notification.CompetentAuthority.Value == competentAuthority.Value
                        from Exporter in context.Exporters
                            .Where(e => Notification.Id == e.NotificationId).DefaultIfEmpty()
                        from Importer in context.Importers
                            .Where(i => Notification.Id == i.NotificationId).DefaultIfEmpty()
                        join ProducerCollection in context.Producers 
                            on Notification.Id equals ProducerCollection.NotificationId
                        join InternalUser in context.InternalUsers 
                            on Notification.UserId.ToString() equals InternalUser.UserId
                        join Assessment in context.NotificationAssessments.Where(a => a.Status == NotificationStatus.NotSubmitted) 
                            on Notification.Id equals Assessment.NotificationApplicationId
                        select new
                        {
                            Notification.Id,
                            Notification.NotificationNumber,
                            Exporter,
                            Importer,
                            ProducerCollection,
                            InternalUser
                        };

            var result = await query.ToArrayAsync();

            return result.Select(x => ExportNotificationOwnerDisplay.Load(x.Id,
                x.NotificationNumber,
                x.Exporter,
                x.Importer,
                x.ProducerCollection.Producers.SingleOrDefault(p => p.IsSiteOfExport),
                x.InternalUser.User.FullName));
        }
    }
}