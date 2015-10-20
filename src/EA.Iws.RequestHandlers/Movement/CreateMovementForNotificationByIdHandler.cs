namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;
    using Domain.NotificationAssessment;
    using System.Linq;

    public class CreateMovementForNotificationByIdHandler : IRequestHandler<CreateMovementForNotificationById, Guid>
    {
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public CreateMovementForNotificationByIdHandler(IwsContext context,
            MovementFactory movementFactory,
            IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepository,
            INotificationAssessmentRepository assessmentRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.context = context;
            this.movementFactory = movementFactory;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationRepository = notificationRepository;
            this.movementRepository = movementRepository;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<Guid> HandleAsync(CreateMovementForNotificationById message)
        {
            var notification = await notificationRepository.GetById(message.Id);
            var notificationAssessment = await assessmentRepository.GetByNotificationId(message.Id);
            var movements = await movementRepository.GetAllMovements(message.Id);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.Id);

            var movement = movementFactory.Create(notification, notificationAssessment, shipmentInfo, movements.ToArray());

            context.Movements.Add(movement);

            await context.SaveChangesAsync();

            return movement.Id;
        }
    }
}
