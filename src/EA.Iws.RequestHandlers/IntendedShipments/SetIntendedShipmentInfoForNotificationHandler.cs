namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Notification;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class SetIntendedShipmentInfoForNotificationHandler : IRequestHandler<SetIntendedShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext context;

        public SetIntendedShipmentInfoForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetIntendedShipmentInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(command.NotificationId);

            var shipmentPeriod = new ShipmentPeriod(
                command.StartDate, 
                command.EndDate, 
                notification.IsPreconsentedRecoveryFacility.GetValueOrDefault());

            var shipmentQuantity = new ShipmentQuantity(
                command.Quantity,
                command.Units);

            if (shipmentInfo == null)
            {
                shipmentInfo = new ShipmentInfo(command.NotificationId, 
                    shipmentPeriod, 
                    command.NumberOfShipments, 
                    shipmentQuantity);

                context.ShipmentInfos.Add(shipmentInfo);
            }
            else
            {
                shipmentInfo.UpdateNumberOfShipments(command.NumberOfShipments);
                shipmentInfo.UpdateShipmentPeriod(shipmentPeriod);
                shipmentInfo.UpdateQuantity(shipmentQuantity);
            }

            await context.SaveChangesAsync();

            return shipmentInfo.Id;
        }
    }
}