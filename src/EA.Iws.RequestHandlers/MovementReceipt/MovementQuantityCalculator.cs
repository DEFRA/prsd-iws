namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.MovementReceipt;

    internal class MovementQuantityCalculator : IMovementQuantityCalculator
    {
        private readonly IwsContext context;
        private readonly IMovementReceiptService movementReceiptService;

        public MovementQuantityCalculator(IwsContext context, IMovementReceiptService movementReceiptService)
        {
            this.context = context;
            this.movementReceiptService = movementReceiptService;
        }

        public async Task<decimal> TotalQuantityReceivedAsync(Guid notificationId)
        {
            var movements = await context.Movements.Where(m =>
                    m.NotificationApplicationId == notificationId)
                .ToListAsync();

            return movements.Where(m => movementReceiptService.IsReceived(m))
                .Sum(m => m.Receipt.Quantity.Value);
        }

        public async Task<decimal> TotalQuantityRemainingAsync(Guid notificationId)
        {
            var notification = await context.NotificationApplications
                .SingleAsync(na => na.Id == notificationId);

            return notification.ShipmentInfo.Quantity - await TotalQuantityReceivedAsync(notificationId);
        }
    }
}
