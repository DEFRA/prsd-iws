namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetMovementsByNotificationIdHandler : IRequestHandler<GetMovementsByNotificationId, IList<MovementTableDataRow>>
    {
        private readonly IImportMovementRepository repository;
        private readonly IMapper mapper;

        public GetMovementsByNotificationIdHandler(IImportMovementRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IList<MovementTableDataRow>> HandleAsync(GetMovementsByNotificationId message)
        {
            var movements = await repository.GetForNotification(message.Id);

            return movements.Select(m => mapper.Map<MovementTableDataRow>(m)).ToList();
        }
    }
}
