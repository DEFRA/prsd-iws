namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    public class SubmittedMovementMap : IMap<Movement, SubmittedMovement>
    {
        public SubmittedMovement Map(Movement source)
        {
            return new SubmittedMovement
            {
                MovementId = source.Id,
                NotificationId = source.NotificationId,
                Number = source.Number,
                ShipmentDate = source.Date.GetValueOrDefault()
            };
        }
    }
}
