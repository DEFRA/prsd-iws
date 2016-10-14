namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetCancellableMovementsHandler :
        IRequestHandler<GetCancellableMovements, IEnumerable<ImportCancellableMovement>>
    {
        private readonly IMapper mapper;
        private readonly IImportMovementRepository movementRepository;

        public GetCancellableMovementsHandler(IImportMovementRepository movementRepository, IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ImportCancellableMovement>> HandleAsync(GetCancellableMovements message)
        {
            return (await movementRepository.GetPrenotifiedForNotification(message.ImportNotificationId))
                .Where(x => !x.IsCancelled)
                .Select(x => mapper.Map<ImportCancellableMovement>(x));
        }
    }
}