namespace EA.Iws.RequestHandlers.NotificationMovements.Edit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Edit;

    internal class GetEditableMovementsHandler : IRequestHandler<GetEditableMovements, IEnumerable<MovementData>>
    {
        private readonly IMapper mapper;
        private readonly IMovementRepository movementRepository;

        public GetEditableMovementsHandler(IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovementData>> HandleAsync(GetEditableMovements message)
        {
            var submittedMovements = await movementRepository
                .GetMovementsByStatus(message.NotificationId, MovementStatus.Submitted);

            return submittedMovements
                .Where(x => !x.HasShipped)
                .Select(x => mapper.Map<MovementData>(x))
                .ToArray();
        }
    }
}