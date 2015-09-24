namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GetShipmentDateDataByMovementIdHandler : IRequestHandler<GetShipmentDateDataByMovementId, MovementDatesData>
    {
        private readonly IwsContext context;

        public GetShipmentDateDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementDatesData> HandleAsync(GetShipmentDateDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var notification = await context.GetNotificationApplication(movement.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(movement.NotificationId);

            return new MovementDatesData
            {
                MovementId = message.MovementId,
                FirstDate = shipmentInfo.ShipmentPeriod.FirstDate,
                LastDate = shipmentInfo.ShipmentPeriod.LastDate,
                ActualDate = movement.Date
            };
        }
    }
}
