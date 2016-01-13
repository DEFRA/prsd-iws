namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class ImportNotificationOverviewRepository : IImportNotificationOverviewRepository
    {
        private readonly ImportNotificationContext context;

        public ImportNotificationOverviewRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportNotificationOverview> Get(Guid notificationId)
        {
            var data = await context.ImportNotifications
                .Join(context.ImportNotificationAssessments,
                    n => n.Id,
                    a => a.NotificationApplicationId,
                    (n, a) => new
                    {
                        Notification = n,
                        Assessment = a
                    })
                .Join(context.Exporters,
                    x => x.Notification.Id,
                    e => e.ImportNotificationId,
                    (x, e) => new
                    {
                        Exporter = e,
                        x.Notification,
                        x.Assessment
                    })
                .Join(context.Importers,
                    x => x.Notification.Id,
                    i => i.ImportNotificationId,
                    (x, i) => new
                    {
                        Importer = i,
                        x.Notification,
                        x.Assessment,
                        x.Exporter
                    })
                .Join(context.Producers,
                    x => x.Notification.Id,
                    p => p.ImportNotificationId,
                    (x, p) => new
                    {
                        Producer = p,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer
                    })
                .Join(context.Facilities,
                    x => x.Notification.Id,
                    f => f.ImportNotificationId,
                    (x, f) => new
                    {
                        Facilities = f,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer,
                        x.Producer
                    })
                .Join(context.Shipments,
                    x => x.Notification.Id,
                    s => s.ImportNotificationId,
                    (x, s) => new
                    {
                        Shipment = s,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer,
                        x.Producer,
                        x.Facilities
                    })
                .Join(context.TransportRoutes,
                    x => x.Notification.Id,
                    t => t.ImportNotificationId,
                    (x, t) => new
                    {
                        TransportRoute = t,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer,
                        x.Producer,
                        x.Facilities,
                        x.Shipment
                    })
                .Join(context.OperationCodes,
                    x => x.Notification.Id,
                    o => o.ImportNotificationId,
                    (x, o) => new
                    {
                        WasteOperation = o,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer,
                        x.Producer,
                        x.Facilities,
                        x.TransportRoute,
                        x.Shipment
                    })
                .Join(context.WasteTypes,
                    x => x.Notification.Id,
                    t => t.ImportNotificationId,
                    (x, t) => new
                    {
                        WasteType = t,
                        x.Notification,
                        x.Assessment,
                        x.Exporter,
                        x.Importer,
                        x.Producer,
                        x.Facilities,
                        x.Shipment,
                        x.TransportRoute,
                        x.WasteOperation
                    })
                .SingleAsync(x => x.Notification.Id == notificationId);

            return ImportNotificationOverview.Load(data.Notification,
                data.Assessment,
                data.Exporter,
                data.Importer,
                data.Producer,
                data.Facilities,
                data.Shipment,
                data.TransportRoute,
                data.WasteOperation,
                data.WasteType);
        }

        public Task<ImportNotificationOverview> GetFromDraft(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}