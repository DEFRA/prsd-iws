﻿namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class NotificationApplicationOverviewRepository : INotificationApplicationOverviewRepository
    {
        private readonly INotificationProgressService progressService;
        private readonly INotificationApplicationAuthorization authorization;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly IwsContext db;

        public NotificationApplicationOverviewRepository(
            IwsContext db,
            INotificationChargeCalculator chargeCalculator,
            INotificationProgressService progressService,
            INotificationApplicationAuthorization authorization)
        {
            this.db = db;
            this.chargeCalculator = chargeCalculator;
            this.progressService = progressService;
            this.authorization = authorization;
        }

        public async Task<NotificationApplicationOverview> GetById(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            var query =
                from notification in db.NotificationApplications
                where notification.Id == notificationId && notification.IsArchived == false
                from wasteRecovery
                //left join waste recovery, if it exists
                in db.WasteRecoveries
                    .Where(wr => wr.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from assessment
                //left join assessment, if it exists
                in db.NotificationAssessments
                    .Where(na => na.NotificationApplicationId == notification.Id)
                    .DefaultIfEmpty()
                from wasteDiposal
                in db.WasteDisposals
                    .Where(wd => wd.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from exporter
                    in db.Exporters
                    .Where(e => e.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from importer
                    in db.Importers
                    .Where(i => i.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                from shipmentInfo
                in db.ShipmentInfos
                    .Where(si => si.NotificationId == notification.Id)
                    .DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    WasteRecovery = wasteRecovery,
                    WasteDisposal = wasteDiposal,
                    NotificationAssessment = assessment,
                    ShipmentInfo = shipmentInfo,
                    Exporter = exporter,
                    Importer = importer
                };

            var data = await query.SingleAsync();

            return NotificationApplicationOverview.Load(
                data.Notification,
                data.NotificationAssessment,
                data.WasteRecovery,
                data.WasteDisposal,
                data.Exporter,
                data.Importer,
                decimal.ToInt32(await chargeCalculator.GetValue(notificationId)), 
                progressService.GetNotificationProgressInfo(notificationId));
        }
    }
}
