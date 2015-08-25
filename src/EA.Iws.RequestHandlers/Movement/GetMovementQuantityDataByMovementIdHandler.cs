namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
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

            var notificationQuantityAndUnits =
                await context.NotificationApplications
                .Where(na => na.Id == movement.NotificationApplicationId)
                .Select(na => new { na.ShipmentInfo.Quantity, na.ShipmentInfo.Units }).SingleAsync();

            var currentlyUsed = await context.Movements
                .Where(m => m.NotificationApplicationId == movement.NotificationApplicationId
                && m.Quantity.HasValue)
                .SumAsync(m => m.Quantity);

            return new MovementQuantityData
            {
                TotalNotifiedQuantity = notificationQuantityAndUnits.Quantity,
                ThisMovementQuantity = movement.Quantity,
                TotalCurrentlyUsedQuantity = currentlyUsed.GetValueOrDefault(),
                Units = notificationQuantityAndUnits.Units
            };
        }
    }
}
