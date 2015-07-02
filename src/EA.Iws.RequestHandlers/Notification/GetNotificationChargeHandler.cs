namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationChargeHandler : IRequestHandler<GetNotificationCharge, decimal>
    {
        private readonly IwsContext context;

        public GetNotificationChargeHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<decimal> HandleAsync(GetNotificationCharge message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            if (!notification.HasShipmentInfo)
            {
                return 0;
            }

            var numberOfShipments = notification.ShipmentInfo.NumberOfShipments;

            // TODO: hard coded to Export for now, need to read TradeDirection from Notification when implemented
            return (await context.PricingStructures.SingleAsync(
                p => p.CompetentAuthority == notification.CompetentAuthority &&
                     p.Activity.TradeDirection == TradeDirection.Export &&
                     p.Activity.NotificationType == notification.NotificationType &&
                     p.Activity.IsInterim == notification.IsInterim &&
                     (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments &&
                      (p.ShipmentQuantityRange.RangeTo == null ||
                       p.ShipmentQuantityRange.RangeTo >= numberOfShipments)))).Price;
        }
    }
}