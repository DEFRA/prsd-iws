namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly INotificationApplicationRepository notificationRepositoy;
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly ActiveMovements activeMovementService;
        private readonly NotificationMovementsQuantity movementQuantityCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IwsContext context,
            ActiveMovements activeMovementService,
            NotificationMovementsQuantity movementQuantityCalculator,
            IShipmentInfoRepository shipmentInfoRepository,
            IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepositoy)
        {
            this.context = context;
            this.activeMovementService = activeMovementService;
            this.movementQuantityCalculator = movementQuantityCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.movementRepository = movementRepository;
            this.notificationRepositoy = notificationRepositoy;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await movementRepository
                .GetById(message.Id);

            var notification = await notificationRepositoy
                .GetByMovementId(message.Id);

            var relatedMovements = await movementRepository
                .GetAllMovements(notification.Id);

            var financialGuarantee = await context.FinancialGuarantees
                .SingleAsync(fg => 
                    fg.NotificationApplicationId == movement.NotificationId);

            var shipmentInfo = await shipmentInfoRepository
                .GetByNotificationId(movement.NotificationId);

            return new MovementReceiptSummaryData
            {
                NotificationId = movement.NotificationId,
                MovementId = movement.Id,
                NotificationNumber = notification.NotificationNumber,
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                CurrentActiveLoads = activeMovementService.Total(relatedMovements.ToList()),
                QuantitySoFar = await movementQuantityCalculator.Received(movement.NotificationId),
                QuantityRemaining = await movementQuantityCalculator
                    .Remaining(movement.NotificationId),
                DisplayUnit = shipmentInfo.Units
            };
        }
    }
}
