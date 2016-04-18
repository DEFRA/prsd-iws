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
        private readonly IMap<Movement, MovementData> mapper;

        public GetSubmittedMovementsByNotificationIdHandler(
            IMovementRepository movementRepository,
            IMap<Movement, MovementData> mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetSubmittedMovementsByNotificationId message)
        {
            var movements = await movementRepository.GetMovementsByStatus(message.Id, MovementStatus.Submitted);

            return movements.Where(m => m.HasShipped).Select(mapper.Map).ToArray();
        }
    }
}
