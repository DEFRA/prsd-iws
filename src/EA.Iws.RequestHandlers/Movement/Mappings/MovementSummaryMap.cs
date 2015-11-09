namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    public class MovementSummaryMap : IMapWithParameter<NotificationMovementsSummary, Movement, MovementSummary>
    {
        private readonly IMapper mapper;

        public MovementSummaryMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public MovementSummary Map(NotificationMovementsSummary source, Movement parameter)
        {
            return new MovementSummary
            {
                SummaryData = mapper.Map<BasicMovementSummary>(source),
                MovementId = parameter.Id,
                MovementNumber = parameter.Number
            };
        }
    }
}