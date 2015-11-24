namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class NotificationApplicationOverviewRepository : INotificationApplicationOverviewRepository
    {
        private readonly INotificationProgressService progressService;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly IwsContext db;

        public NotificationApplicationOverviewRepository(
            IwsContext db,
            INotificationChargeCalculator chargeCalculator,
            INotificationProgressService progressService)
        {
            this.db = db;
            this.chargeCalculator = chargeCalculator;
            this.progressService = progressService;
        }

        public async Task<NotificationApplicationOverview> GetById(Guid notificationId)
        {
            var query =
                from notification in db.NotificationApplications
                where notification.Id == notificationId
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
                    Exporter = exporter
                };

            var data = await query.SingleAsync();

            return NotificationApplicationOverview.Load(
                data.Notification,
                data.NotificationAssessment,
                data.WasteRecovery,
                data.WasteDisposal,
                data.Exporter,
                decimal.ToInt32(await chargeCalculator.GetValue(notificationId)), 
                progressService.GetNotificationProgressInfo(notificationId));
        }
    }
}
