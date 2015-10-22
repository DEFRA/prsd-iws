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

    internal class GetSubmittedMovementsHandler :
        IRequestHandler<GetSubmittedMovements, List<SubmittedMovement>>
    {
        private readonly IMap<Movement, SubmittedMovement> mapper;
        private readonly IMovementRepository movementRepository;

        public GetSubmittedMovementsHandler(IMovementRepository movementRepository,
            IMap<Movement, SubmittedMovement> mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<List<SubmittedMovement>> HandleAsync(GetSubmittedMovements message)
        {
            var movements = await movementRepository.GetSubmittedMovements(message.NotificationId);

            return movements.Select(m => mapper.Map(m)).ToList();
        }
    }
}