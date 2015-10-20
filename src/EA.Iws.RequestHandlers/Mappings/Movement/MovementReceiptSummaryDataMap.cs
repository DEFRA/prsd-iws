namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    public class MovementReceiptSummaryDataMap : IMapWithParameter<NotificationMovementsSummary, Movement, MovementReceiptSummaryData>
    {
        public MovementReceiptSummaryData Map(NotificationMovementsSummary source, Movement parameter)
        {
            return new MovementReceiptSummaryData
            {
                NotificationId = source.NotificationId,
                NotificationNumber = source.NotificationNumber,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                CurrentActiveLoads = source.CurrentActiveLoads,
                QuantitySoFar = source.QuantityReceived,
                QuantityRemaining = source.QuantityRemaining,
                DisplayUnit = source.Units,
                MovementId = parameter.Id,
                ThisMovementNumber = parameter.Number
            };
        }
    }
}
