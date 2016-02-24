namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.IntendedShipments;
    using Domain.NotificationApplication.Shipment;
    using Domain.Security;

    internal class ShipmentInfoRepository : IShipmentInfoRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public ShipmentInfoRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<ShipmentInfo> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.ShipmentInfos.SingleOrDefaultAsync(si => si.NotificationId == notificationId);
        }

        public async Task<IntendedShipmentData> GetIntendedShipmentDataByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            var data = await context.ShipmentInfos.Where(s => s.NotificationId == notificationId)
                .Join(context.Facilities,
                    s => s.NotificationId,
                    f => f.NotificationId, (s, f) => new { s, f })
                .Join(context.NotificationAssessments,
                    x => x.s.NotificationId,
                    a => a.NotificationApplicationId,
                    (x, a) => new { Shipment = x.s, FacilityCollection = x.f, a.Status })
                .SingleAsync();

            return new IntendedShipmentData
            {
                NotificationId = notificationId,
                Units = data.Shipment.Units,
                FirstDate = data.Shipment.ShipmentPeriod.FirstDate,
                LastDate = data.Shipment.ShipmentPeriod.LastDate,
                HasShipmentData = true,
                IsPreconsentedRecoveryFacility = data.FacilityCollection.AllFacilitiesPreconsented.GetValueOrDefault(),
                NumberOfShipments = data.Shipment.NumberOfShipments,
                Quantity = data.Shipment.Quantity,
                Status = data.Status
            };
        }
    }
}
