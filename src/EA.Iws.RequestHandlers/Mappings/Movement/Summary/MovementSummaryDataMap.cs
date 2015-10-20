namespace EA.Iws.RequestHandlers.Mappings.Movement.Summary
{
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementSummaryDataMap : IMapWithParameter<NotificationMovementsSummary, Movement[], MovementSummaryData>
    {
        private readonly IMapper mapper;

        public MovementSummaryDataMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public MovementSummaryData Map(NotificationMovementsSummary source, Movement[] parameter)
        {
            return new MovementSummaryData
            {
                NotificationId = source.NotificationId,
                NotificationNumber = source.NotificationNumber,
                NotificationType = source.NotificationType,
                IntendedShipments = source.IntendedTotalShipments,
                UsedShipments = source.CurrentTotalShipments,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                ActiveLoadsCurrent = source.CurrentActiveLoads,
                IntendedQuantityTotal = source.IntendedQuantity,
                ReceivedQuantityTotal = source.QuantityReceived,
                DisplayUnits = source.Units,
                ShipmentTableData = parameter.Select(p => mapper.Map<MovementSummaryTableData>(p)).ToList()
            };
        }
    }
}
