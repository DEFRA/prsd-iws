namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly IwsContext context;
        private readonly IActiveMovementsService activeMovements;
        private readonly IMovementQuantityCalculator movementQuantity;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IwsContext context, 
            IActiveMovementsService activeMovementsService,
            IMovementQuantityCalculator movementQuantityCalculator)
        {
            this.context = context;
            this.activeMovements = activeMovementsService;
            this.movementQuantity = movementQuantityCalculator;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);
            
            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == movement.NotificationApplicationId);

            var data = new MovementReceiptSummaryData
            {
                NotificationId = movement.NotificationApplicationId,
                MovementId = movement.Id,
                NotificationNumber = movement.NotificationApplication.NotificationNumber,
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                CurrentActiveLoads = await activeMovements.TotalActiveMovementsAsync(movement.NotificationApplicationId),
                QuantitySoFar = await movementQuantity.TotalQuantityReceivedAsync(movement.NotificationApplicationId),
                QuantityRemaining = await movementQuantity.TotalQuantityRemainingAsync(movement.NotificationApplicationId),
                DisplayUnit = movement.NotificationApplication.ShipmentInfo.Units
            };

            return data;
        }
    }
}
