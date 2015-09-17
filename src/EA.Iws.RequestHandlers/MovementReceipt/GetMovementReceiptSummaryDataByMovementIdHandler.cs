namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Prsd.Core.Mediator;
    using RequestHandlers.Movement;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly IwsContext context;
        private readonly ActiveMovementCalculator activeMovementCalculator;
        private readonly MovementQuantityCalculator movementQuantityCalculator;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IwsContext context, 
            ActiveMovementCalculator activeMovementCalculator,
            MovementQuantityCalculator movementQuantityCalculator)
        {
            this.context = context;
            this.activeMovementCalculator = activeMovementCalculator;
            this.movementQuantityCalculator = movementQuantityCalculator;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await context
                .Movements.SingleAsync(m => m.Id == message.Id);

            var relatedMovements = await context
                .GetMovementsForNotificationAsync(movement.NotificationApplicationId);

            var financialGuarantee = await context
                .FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == movement.NotificationApplicationId);

            var data = new MovementReceiptSummaryData
            {
                NotificationId = movement.NotificationApplicationId,
                MovementId = movement.Id,
                NotificationNumber = movement.NotificationApplication.NotificationNumber,
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                CurrentActiveLoads = activeMovementCalculator.TotalActiveMovements(relatedMovements),
                QuantitySoFar = movementQuantityCalculator.QuantityReceived(relatedMovements),
                QuantityRemaining = movementQuantityCalculator
                    .QuantityRemaining(movement.NotificationApplication.ShipmentInfo, relatedMovements),
                DisplayUnit = movement.NotificationApplication.ShipmentInfo.Units
            };

            return data;
        }
    }
}
