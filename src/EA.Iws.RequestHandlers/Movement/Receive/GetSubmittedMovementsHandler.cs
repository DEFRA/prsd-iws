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

    internal class GetSubmittedMovementsHandler :
        IRequestHandler<GetSubmittedMovements, List<SubmittedMovement>>
    {
        private readonly IMapper mapper;
        private readonly IMovementRepository movementRepository;

        public GetSubmittedMovementsHandler(IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<List<SubmittedMovement>> HandleAsync(GetSubmittedMovements message)
        {
            var movements = await movementRepository.GetMovementsByStatus(message.NotificationId, MovementStatus.Submitted);

            return movements.Select(m => mapper.Map<SubmittedMovement>(m)).OrderBy(m => m.Number).ToList();
        }
    }
}