namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class NotificationMovementsSummaryAndTableMap : IMapWithParameter<NotificationMovementsSummary, Movement[], NotificationMovementsSummaryAndTable>
    {
        private readonly IMapper mapper;

        public NotificationMovementsSummaryAndTableMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public NotificationMovementsSummaryAndTable Map(NotificationMovementsSummary source, Movement[] parameter)
        {
            return new NotificationMovementsSummaryAndTable
            {
                SummaryData = mapper.Map<BasicMovementSummary>(source),
                NotificationType = source.NotificationType,
                TotalIntendedShipments = source.IntendedTotalShipments,
                ShipmentTableData = parameter.Select(p => mapper.Map<MovementTableDataRow>(p)).ToList()
            };
        }
    }
}
