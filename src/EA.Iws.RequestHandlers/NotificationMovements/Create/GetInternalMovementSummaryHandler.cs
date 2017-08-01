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

            return new InternalMovementSummary
            {
                NotificationId = summary.NotificationId,
                NotificationNumber = summary.NotificationNumber,
                TotalShipments = summary.CurrentTotalShipments,
                QuantityReceived = summary.QuantityReceived,
                QuantityRemaining = summary.QuantityRemaining,
                DisplayUnit = summary.Units,
                ActiveLoadsPermitted = summary.ActiveLoadsPermitted,
                CurrentActiveLoads = summary.CurrentActiveLoads,
                CompetentAuthority = summary.CompetentAuthority,
                FinancialGuaranteeStatus = summary.FinancialGuaranteeStatus,
                NotificationStatus = summary.NotificationStatus,
                TotalIntendedShipments = summary.IntendedTotalShipments,
                AverageTonnage = summary.AverageTonnage,
                AverageDataUnit = summary.AverageDataUnit
            };
        }
    }
}
