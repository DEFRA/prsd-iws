namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetSubmittedMovementsByNotificationIdHandler : IRequestHandler<GetSubmittedMovementsByNotificationId, IList<MovementData>>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMap<Movement, MovementData> mapper;
        private readonly ActiveMovements activeMovementService;

        public GetSubmittedMovementsByNotificationIdHandler(
            IMovementRepository movementRepository,
            IMap<Movement, MovementData> mapper,
            ActiveMovements activeMovementService)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
            this.activeMovementService = activeMovementService;
        }

        public async Task<IList<MovementData>> HandleAsync(GetSubmittedMovementsByNotificationId message)
        {
            var movements = await movementRepository.GetSubmittedMovements(message.Id);

            return activeMovementService.List(movements.ToList())
                .Select(mapper.Map)
                .ToArray();
        }
    }
}
