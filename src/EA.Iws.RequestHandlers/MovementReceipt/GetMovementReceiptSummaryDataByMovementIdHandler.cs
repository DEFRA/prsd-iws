namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly IwsContext context;
        private readonly ActiveMovements activeMovementService;
        private readonly MovementQuantity movementQuantityCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IwsContext context, 
            ActiveMovements activeMovementService,
            MovementQuantity movementQuantityCalculator,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.activeMovementService = activeMovementService;
            this.movementQuantityCalculator = movementQuantityCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await context
                .Movements.SingleAsync(m => m.Id == message.Id);

            var notification = await context
                .GetNotificationApplication(movement.NotificationId);

            var relatedMovements = await context
                .GetMovementsForNotificationAsync(movement.NotificationId);

            var financialGuarantee = await context
                .FinancialGuarantees.SingleAsync(fg => fg.NotificationApplicationId == movement.NotificationId);

            var shipmentInfo = await shipmentInfoRepository
                .GetByNotificationId(movement.NotificationId);

            return new MovementReceiptSummaryData
            {
                NotificationId = movement.NotificationId,
                MovementId = movement.Id,
                NotificationNumber = notification.NotificationNumber,
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                CurrentActiveLoads = activeMovementService.Total(relatedMovements),
                QuantitySoFar = movementQuantityCalculator.Received(shipmentInfo, relatedMovements),
                QuantityRemaining = movementQuantityCalculator
                    .Remaining(shipmentInfo, relatedMovements),
                DisplayUnit = shipmentInfo.Units
            };
        }
    }
}
