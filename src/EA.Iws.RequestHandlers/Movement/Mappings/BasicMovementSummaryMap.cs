namespace EA.Iws.RequestHandlers.Movement.Mappings
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class BasicMovementSummaryMap : IMap<NotificationMovementsSummary, BasicMovementSummary>
    {
        public BasicMovementSummary Map(NotificationMovementsSummary source)
        {
            return new BasicMovementSummary
            {
                NotificationId = source.NotificationId,
                NotificationNumber = source.NotificationNumber,
                TotalShipments = source.CurrentTotalShipments,
                QuantityReceived = source.QuantityReceived,
                QuantityRemaining = source.QuantityRemaining,
                DisplayUnit = source.Units,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                CurrentActiveLoads = source.CurrentActiveLoads
            };
        }
    }
}