namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    public class CreateMovementForNotificationByIdHandler : IRequestHandler<CreateMovementForNotificationById, Guid>
    {
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public CreateMovementForNotificationByIdHandler(IwsContext context, 
            MovementFactory movementFactory, 
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.movementFactory = movementFactory;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<Guid> HandleAsync(CreateMovementForNotificationById message)
        {
            var notification = await context.GetNotificationApplication(message.Id);
            var notificationAssessment =
                await context.NotificationAssessments.SingleAsync(na => na.NotificationApplicationId == message.Id);
            var movements = await context.GetMovementsForNotificationAsync(message.Id);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.Id);

            var movement = movementFactory.Create(notification, notificationAssessment, shipmentInfo, movements);

            context.Movements.Add(movement);

            await context.SaveChangesAsync();

            return movement.Id;
        }
    }
}
