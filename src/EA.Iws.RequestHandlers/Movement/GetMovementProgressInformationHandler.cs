namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementProgressInformationHandler : IRequestHandler<GetMovementProgressInformation, MovementProgressAndSummaryData>
    {
        private readonly IMapper mapper;
        private readonly IMovementRepository movementRepository;
        private readonly INotificationMovementsSummaryRepository summaryRepository;

        public GetMovementProgressInformationHandler(
            INotificationMovementsSummaryRepository summaryRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.summaryRepository = summaryRepository;
            this.movementRepository = movementRepository;
            this.mapper = mapper;
        }

        public async Task<MovementProgressAndSummaryData> HandleAsync(GetMovementProgressInformation message)
        {
            var movement = await movementRepository.GetById(message.MovementId);
            var summaryData = await summaryRepository.GetById(movement.NotificationId);

            return mapper.Map<NotificationMovementsSummary, Movement, MovementProgressAndSummaryData>(summaryData, movement);
        }
    }
}