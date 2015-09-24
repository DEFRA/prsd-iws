namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GetMovementProgressInformationHandler : IRequestHandler<GetMovementProgressInformation, MovementProgressAndSummaryData>
    {
        private readonly IwsContext context;
        private readonly IMap<Movement, ProgressData> progressMap;
        private readonly ActiveMovements activeMovementService;

        public GetMovementProgressInformationHandler(IwsContext context, 
            IMap<Movement, ProgressData> progressMap,
            ActiveMovements activeMovementService)
        {
            this.context = context;
            this.progressMap = progressMap;
            this.activeMovementService = activeMovementService;
        }

        public async Task<MovementProgressAndSummaryData> HandleAsync(GetMovementProgressInformation message)
        {
            var movement = await context.Movements
                .SingleAsync(m => m.Id == message.MovementId);
            var notificationId = movement.NotificationId;
            var relatedMovements = await context.GetMovementsForNotificationAsync(notificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(notificationId);

            var notificationInformation = await context.NotificationApplications
                .Join(context.FinancialGuarantees,
                application => application.Id == notificationId,
                fg => fg.NotificationApplicationId == notificationId,
                (na, fg) => new
                {
                    Id = na.Id,
                    Number = na.NotificationNumber, 
                    Shipments = shipmentInfo.NumberOfShipments,
                    ActiveLoadsPermitted = fg.ActiveLoadsPermitted
                })
                .SingleAsync(x => x.Id == notificationId);

            return new MovementProgressAndSummaryData
            {
                NotificationNumber = notificationInformation.Number,
                TotalNumberOfMovements = notificationInformation.Shipments,
                CurrentActiveLoads = activeMovementService.Total(relatedMovements),
                ThisMovementNumber = movement.Number,
                ActiveLoadsPermitted = notificationInformation.ActiveLoadsPermitted.GetValueOrDefault(),
                Progress = progressMap.Map(movement),
                NotificationId = notificationInformation.Id,
                MovementId = movement.Id
            };
        }
    }
}
