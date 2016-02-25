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

            var facilityStatusData = await context.Facilities.Where(f => f.NotificationId == notificationId)
                .Join(context.NotificationAssessments,
                    f => f.NotificationId,
                    a => a.NotificationApplicationId,
                    (f, a) => new { IsPreconsented = f.AllFacilitiesPreconsented, a.Status })
                    .SingleAsync();

            var shipment = await context.ShipmentInfos.SingleOrDefaultAsync(s => s.NotificationId == notificationId);

            if (shipment == null)
            {
                return new IntendedShipmentData
                {
                    Status = facilityStatusData.Status,
                    NotificationId = notificationId,
                    IsPreconsentedRecoveryFacility = facilityStatusData.IsPreconsented.GetValueOrDefault(),
                    HasShipmentData = false
                };
            }

            return new IntendedShipmentData
            {
                NotificationId = notificationId,
                Units = shipment.Units,
                FirstDate = shipment.ShipmentPeriod.FirstDate,
                LastDate = shipment.ShipmentPeriod.LastDate,
                HasShipmentData = true,
                IsPreconsentedRecoveryFacility = facilityStatusData.IsPreconsented.GetValueOrDefault(),
                NumberOfShipments = shipment.NumberOfShipments,
                Quantity = shipment.Quantity,
                Status = facilityStatusData.Status
            };
        }
    }
}
