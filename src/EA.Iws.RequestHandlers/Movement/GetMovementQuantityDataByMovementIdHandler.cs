namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GetMovementQuantityDataByMovementIdHandler : IRequestHandler<GetMovementQuantityDataByMovementId, MovementQuantityData>
    {
        private readonly IwsContext context;

        public GetMovementQuantityDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementQuantityData> HandleAsync(GetMovementQuantityDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);
            var shipmentInfo = await context.GetShipmentInfoAsync(movement.NotificationId);

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
