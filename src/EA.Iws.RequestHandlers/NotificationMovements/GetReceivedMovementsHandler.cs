namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.MovementOperation;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetReceivedMovementsHandler : IRequestHandler<GetReceivedMovements, MovementOperationData>
    {
        private readonly IMapper mapper;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IMovementRepository movementRepository;

        public GetReceivedMovementsHandler(IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
        }

        public async Task<MovementOperationData> HandleAsync(GetReceivedMovements message)
        {
            var movements = await movementRepository.GetReceivedMovements(message.NotificationId);
            var notification = await notificationRepository.GetById(message.NotificationId);

            var movementsData = movements.Select(m => mapper.Map<MovementData>(m)).ToArray();

            return new MovementOperationData
            {
                MovementDatas = movementsData,
                NotificationType = notification.NotificationType
            };
        }
    }
}
