namespace EA.Iws.RequestHandlers.Movement.Receive
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;

    internal class GetSubmittedMovementsByNotificationIdHandler : IRequestHandler<GetSubmittedMovementsByNotificationId, IList<MovementData>>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;

        public GetSubmittedMovementsByNotificationIdHandler(
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetSubmittedMovementsByNotificationId message)
        {
            var movements = await movementRepository.GetAllActiveMovementsForReceiptAndReceiptRecovery(message.Id);

            return movements.Select(m => mapper.Map<MovementData>(m)).ToList();
        }
    }
}
