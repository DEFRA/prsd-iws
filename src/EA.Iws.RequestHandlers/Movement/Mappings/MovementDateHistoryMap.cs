namespace EA.Iws.RequestHandlers.Movement.Mappings
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementDateHistoryMap : IMap<MovementDateHistory, DateHistory>
    {
        public DateHistory Map(MovementDateHistory source)
        {
            return new DateHistory
            {
                DateChanged = source.DateChanged,
                PreviousDate = source.PreviousDate
            };
        }
    }
}