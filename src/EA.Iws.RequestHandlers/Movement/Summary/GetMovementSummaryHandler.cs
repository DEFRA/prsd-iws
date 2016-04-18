namespace EA.Iws.RequestHandlers.Movement.Summary
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementSummaryHandler : IRequestHandler<GetMovementSummary, MovementSummary>
    {
        private readonly IMapper mapper;
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IMovementRepository movementRepository;

        public GetMovementSummaryHandler(IMapper mapper,
            IMovementRepository movementRepository,
            INotificationMovementsSummaryRepository summaryRepository)
        {
            this.movementRepository = movementRepository;
            this.summaryRepository = summaryRepository;
            this.mapper = mapper;
        }

        public async Task<MovementSummary> HandleAsync(GetMovementSummary message)
        {
            var movement = await movementRepository
                .GetById(message.Id);

            var summaryData = await summaryRepository
                .GetById(movement.NotificationId);

            return mapper.Map<MovementSummary>(summaryData, movement);
        }
    }
}
