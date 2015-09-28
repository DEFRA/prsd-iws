namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GetMovementQuantityDataByMovementIdHandler : IRequestHandler<GetMovementQuantityDataByMovementId, MovementQuantityData>
    {
        private readonly IwsContext context;
        private readonly IShipmentInfoRepository repository;

        public GetMovementQuantityDataByMovementIdHandler(IwsContext context, IShipmentInfoRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<MovementQuantityData> HandleAsync(GetMovementQuantityDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);
            var shipmentInfo = await repository.GetByNotificationId(movement.NotificationId);

            var currentlyUsed = await context.Movements
                .Where(m => m.NotificationId == movement.NotificationId
                && m.Quantity.HasValue)
                .SumAsync(m => m.Quantity);

            return new MovementQuantityData
            {
                TotalNotifiedQuantity = shipmentInfo.Quantity,
                ThisMovementQuantity = movement.Quantity,
                TotalCurrentlyUsedQuantity = currentlyUsed.GetValueOrDefault(),
                Units = movement.Units ?? shipmentInfo.Units
            };
        }
    }
}
