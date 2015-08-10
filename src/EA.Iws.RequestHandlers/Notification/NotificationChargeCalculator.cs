namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.NotificationApplication;

    internal class NotificationChargeCalculator : INotificationChargeCalculator
    {
        private readonly IwsContext context;

        public NotificationChargeCalculator(IwsContext context)
        {
            this.context = context;
        }

        public async Task<decimal> GetValue(Guid notificationId)
        {
            var notification = await context.GetNotificationApplication(notificationId);

            if (!notification.HasShipmentInfo)
            {
                return 0;
            }

            // Properties must be evaluated before the next query due to lazy loading.
            var numberOfShipments = notification.ShipmentInfo.NumberOfShipments;
            var competentAuthority = notification.CompetentAuthority.Value;
            var notificationType = notification.NotificationType.Value;
            var isInterim = notification.IsInterim;

            // TODO: hard coded to Export for now, need to read TradeDirection from Notification when implemented
            var pricingStructure = await context.PricingStructures.SingleAsync(
                p => p.CompetentAuthority.Value == competentAuthority &&
                     p.Activity.TradeDirection == TradeDirection.Export &&
                     p.Activity.NotificationType.Value == notificationType &&
                     p.Activity.IsInterim == isInterim &&
                     (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                      (p.ShipmentQuantityRange.RangeTo == null ||
                       p.ShipmentQuantityRange.RangeTo >= numberOfShipments)));
            
            return pricingStructure.Price;
        }
    }
}