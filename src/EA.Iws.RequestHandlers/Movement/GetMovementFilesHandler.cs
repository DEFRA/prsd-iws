namespace EA.Iws.RequestHandlers.Movement
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementFilesHandler : IRequestHandler<GetMovementFiles, MovementFiles>
    {
        private const int PageSize = 30;
        private readonly IMapper mapper;
        private readonly IMovementRepository repository;

        public GetMovementFilesHandler(IMovementRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<MovementFiles> HandleAsync(GetMovementFiles message)
        {
            var movements = await repository.GetPagedMovements(message.NotificationId, message.PageNumber, PageSize);

            var fileData = movements.Select(m => mapper.Map<MovementFileData>(m)).ToArray();
            var numberOfShipments = await repository.GetTotalNumberOfMovements(message.NotificationId, null);

            return new MovementFiles
            {
                FileData = fileData,
                NumberOfShipments = numberOfShipments,
                PageNumber = message.PageNumber,
                PageSize = PageSize
            };
        }
    }
}