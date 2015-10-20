namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementSummaryAndProgressMap : IMapWithParameter<NotificationMovementsSummary, Movement, MovementProgressAndSummaryData>
    {
        private readonly IMapper mapper;

        public MovementSummaryAndProgressMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public MovementProgressAndSummaryData Map(NotificationMovementsSummary source, Movement parameter)
        {
            return new MovementProgressAndSummaryData
            {
                NotificationId = source.NotificationId,
                NotificationNumber = source.NotificationNumber,
                TotalNumberOfMovements = source.CurrentTotalShipments,
                CurrentActiveLoads = source.CurrentActiveLoads,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                MovementId = parameter.Id,
                ThisMovementNumber = parameter.Number,
                Progress = mapper.Map<ProgressData>(parameter)
            };
        }
    }
}
