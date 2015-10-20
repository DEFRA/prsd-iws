namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    public class MovementMap : IMap<Movement, MovementData>
    {
        public MovementData Map(Movement source)
        {
            return new MovementData
            {
                Id = source.Id,
                Number = source.Number
            };
        }
    }
}
