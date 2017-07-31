namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetInternalMovementSummaryHandler : IRequestHandler<GetInternalMovementSummary, InternalMovementSummary>
    {
        private readonly INotificationMovementsSummaryRepository repository;
        private readonly IMapper mapper;
        private readonly NotificationMovementsQuantity quantity;

        public GetInternalMovementSummaryHandler(
            INotificationMovementsSummaryRepository repository, IMapper mapper, NotificationMovementsQuantity quantity)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.quantity = quantity;
        }

        public async Task<InternalMovementSummary> HandleAsync(GetInternalMovementSummary message)
        {
            var summary = await repository.GetById(message.NotificationId);

            //Average tonnage
            var averageData = await quantity.AveragePerShipment(message.NotificationId, summary.IntendedTotalShipments);

            return new InternalMovementSummary
            {
                SummaryData = mapper.Map<BasicMovementSummary>(summary),
                TotalIntendedShipments = summary.IntendedTotalShipments,
                AverageTonnage = averageData.Quantity,
                AverageDataUnit = averageData.Units
            };
        }
    }
}
