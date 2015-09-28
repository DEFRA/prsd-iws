namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GetShipmentDateDataByMovementIdHandler : IRequestHandler<GetShipmentDateDataByMovementId, MovementDatesData>
    {
        private readonly IwsContext context;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetShipmentDateDataByMovementIdHandler(IwsContext context, IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<MovementDatesData> HandleAsync(GetShipmentDateDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(movement.NotificationId);

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
