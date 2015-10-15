namespace EA.Iws.RequestHandlers.Movement.Summary
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement.Summary;

    internal class GetMovementsSummaryByNotificationIdHandler : IRequestHandler<GetMovementsSummaryByNotificationId, MovementSummaryData>
    {
        private readonly IwsContext context;
        private readonly ActiveMovements activeMovementService;
        private readonly MovementQuantity movementQuantityCalculator;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;

        public GetMovementsSummaryByNotificationIdHandler(IwsContext context,
            ActiveMovements activeMovementService,
            MovementQuantity movementQuantityCalculator,
            IShipmentInfoRepository shipmentInfoRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.context = context;
            this.activeMovementService = activeMovementService;
            this.movementQuantityCalculator = movementQuantityCalculator;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<MovementSummaryData> HandleAsync(GetMovementsSummaryByNotificationId message)
        {
            var notification = await notificationApplicationRepository.GetById(message.Id);

            var relatedMovements = (await movementRepository.GetAllMovements(message.Id)).ToList();

            var financialGuarantee = await context.FinancialGuarantees.SingleAsync(
                fg => fg.NotificationApplicationId == message.Id);

            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.Id);

            return new MovementSummaryData
            {
                NotificationId = message.Id,
                NotificationNumber = notification.NotificationNumber,
                NotificationType = (Core.Shared.NotificationType)notification.NotificationType,
                IntendedShipments = shipmentInfo.NumberOfShipments,
                UsedShipments = relatedMovements.Count,
                IntendedQuantityTotal = shipmentInfo.Quantity,
                ReceivedQuantityTotal = movementQuantityCalculator.Received(shipmentInfo, relatedMovements),
                DisplayUnits = shipmentInfo.Units,
                ActiveLoadsPermitted = financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                ActiveLoadsCurrent = activeMovementService.Total(relatedMovements),
                ShipmentTableData = relatedMovements.Select(m => mapper.Map<MovementSummaryTableData>(m)).ToList()
            };
        }
    }
}
