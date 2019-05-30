namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    internal class GetSubmittedPendingMovementsHandler : IRequestHandler<GetSubmittedPendingMovements, List<SubmittedMovement>>
    {
        private readonly IMapper mapper;
        private readonly IMovementRepository movementRepository;

        public GetSubmittedPendingMovementsHandler(IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<List<SubmittedMovement>> HandleAsync(GetSubmittedPendingMovements message)
        {
            var movements = await movementRepository.GetCancellableMovements(message.NotificationId);

            return movements.Select(m => mapper.Map<SubmittedMovement>(m)).OrderBy(m => m.Number).ToList();
        }
    }
}
