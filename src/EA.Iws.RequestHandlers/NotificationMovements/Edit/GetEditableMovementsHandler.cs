namespace EA.Iws.RequestHandlers.NotificationMovements.Edit
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Edit;

    internal class GetEditableMovementsHandler : IRequestHandler<GetEditableMovements, IList<MovementData>>
    {
        private readonly IMapper mapper;
        private readonly ValidMovementDateCalculator movementDateCalculator;
        private readonly IMovementRepository movementRepository;

        public GetEditableMovementsHandler(IMovementRepository movementRepository,
            IMapper mapper,
            ValidMovementDateCalculator movementDateCalculator)
        {
            this.movementRepository = movementRepository;
            this.movementDateCalculator = movementDateCalculator;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetEditableMovements message)
        {
            var submittedMovements = await movementRepository.GetMovementsByStatus(message.NotificationId, MovementStatus.Submitted);

            var result = new List<MovementData>();
            foreach (var movement in submittedMovements)
            {
                var maxAllowedMovementDate = await movementDateCalculator.Maximum(movement.Id);

                if (SystemTime.UtcNow < maxAllowedMovementDate)
                {
                    result.Add(mapper.Map<MovementData>(movement));
                }
            }

            return result;
        }
    }
}