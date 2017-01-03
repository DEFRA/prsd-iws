namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetMovementsByNotificationIdHandler : IRequestHandler<GetMovementsByNotificationId, IList<MovementTableDataRow>>
    {
        private readonly IMovementRepository repository;
        private readonly IMapper mapper;

        public GetMovementsByNotificationIdHandler(IMovementRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementTableDataRow>> HandleAsync(GetMovementsByNotificationId message)
        {
            var movements = await repository.GetAllMovements(message.Id);

            return movements.Select(m => mapper.Map<MovementTableDataRow>(m)).ToList();
        }
    }
}
