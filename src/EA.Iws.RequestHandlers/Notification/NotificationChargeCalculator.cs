namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain.Notification;

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

            var numberOfShipments = notification.ShipmentInfo.NumberOfShipments;

            // TODO: hard coded to Export for now, need to read TradeDirection from Notification when implemented
            return (await context.PricingStructures.SingleAsync(
                p => p.CompetentAuthority.Value == notification.CompetentAuthority.Value &&
                     p.Activity.TradeDirection == TradeDirection.Export &&
                     p.Activity.NotificationType.Value == notification.NotificationType.Value &&
                     p.Activity.IsInterim == notification.IsInterim &&
                     (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                      (p.ShipmentQuantityRange.RangeTo == null ||
                       p.ShipmentQuantityRange.RangeTo >= numberOfShipments)))).Price;
        }
    }
}