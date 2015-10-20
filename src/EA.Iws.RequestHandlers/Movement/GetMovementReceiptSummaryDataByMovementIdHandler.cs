namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementReceiptSummaryDataByMovementIdHandler : IRequestHandler<GetMovementReceiptSummaryDataByMovementId, MovementReceiptSummaryData>
    {
        private readonly IMapper mapper;
        private readonly INotificationMovementsSummaryRepository summaryRepository;
        private readonly IMovementRepository movementRepository;

        public GetMovementReceiptSummaryDataByMovementIdHandler(IMapper mapper,
            IMovementRepository movementRepository,
            INotificationMovementsSummaryRepository summaryRepository)
        {
            this.movementRepository = movementRepository;
            this.summaryRepository = summaryRepository;
            this.mapper = mapper;
        }

        public async Task<MovementReceiptSummaryData> HandleAsync(GetMovementReceiptSummaryDataByMovementId message)
        {
            var movement = await movementRepository
                .GetById(message.Id);

            var summaryData = await summaryRepository
                .GetById(movement.NotificationId);

            return mapper.Map<NotificationMovementsSummary, Movement, MovementReceiptSummaryData>(summaryData, movement);
        }
    }
}
