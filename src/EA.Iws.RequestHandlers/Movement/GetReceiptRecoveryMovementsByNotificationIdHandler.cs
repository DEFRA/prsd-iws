namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using EA.Iws.Requests.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;

    internal class GetReceiptRecoveryMovementsByNotificationIdHandler : IRequestHandler<GetReceiptRecoveryMovementsByNotificationId, IList<MovementData>>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;

        public GetReceiptRecoveryMovementsByNotificationIdHandler(
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetReceiptRecoveryMovementsByNotificationId message)
        {
            var movements = await movementRepository.GetAllActiveMovementsForReceiptAndRecovery(message.NotificationId);

            return movements.Select(m => mapper.Map<MovementData>(m)).ToList();
        }
    }
}
