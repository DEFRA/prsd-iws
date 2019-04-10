namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ImportNotificationOverviewRepository : IImportNotificationOverviewRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationOverviewRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportNotificationOverview> Get(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            var notification =
                await context.ImportNotifications.SingleOrDefaultAsync(x => x.Id == notificationId);
            var assessments =
                await
                    context.ImportNotificationAssessments.SingleOrDefaultAsync(
                        x => x.NotificationApplicationId == notificationId);
            var exporter = await context.Exporters.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var importer = await context.Importers.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var producer = await context.Producers.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var facilities =
                await context.Facilities.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var shipment = await context.Shipments.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var transportRoute =
                await context.TransportRoutes.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var operationCode =
                await context.OperationCodes.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);
            var wasteType =
                await context.WasteTypes.SingleOrDefaultAsync(x => x.ImportNotificationId == notificationId);

            return ImportNotificationOverview.Load(notification,
                assessments,
                exporter,
                importer,
                producer,
                facilities,
                shipment,
                transportRoute,
                operationCode,
                wasteType);
        }

        public Task<ImportNotificationOverview> GetFromDraft(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}