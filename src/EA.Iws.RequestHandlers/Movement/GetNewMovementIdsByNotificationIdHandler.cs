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

    internal class GetNewMovementIdsByNotificationIdHandler : IRequestHandler<GetNewMovementIdsByNotificationId, IList<MovementData>>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMap<Movement, MovementData> mapper;

        public GetNewMovementIdsByNotificationIdHandler(IMovementRepository movementRepository,
            IMap<Movement, MovementData> mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementData>> HandleAsync(GetNewMovementIdsByNotificationId message)
        {
            var movements = await movementRepository.GetMovementsByStatus(message.Id, MovementStatus.New);

            return movements.Select(mapper.Map).ToArray();
        }
    }
}
