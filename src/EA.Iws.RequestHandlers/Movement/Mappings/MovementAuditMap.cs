namespace EA.Iws.RequestHandlers.Movement.Mappings
{
    using Domain.Movement;
    using Prsd.Core.Mapper;
    using Requests.Movement;

    internal class MovementAuditMap : IMap<AuditMovement, MovementAudit>
    {
        public MovementAudit Map(AuditMovement source)
        {
            return new MovementAudit(source.NotificationId, 
                source.ShipmentNumber, 
                source.UserId, 
                (int)source.Type,
                source.DateAdded);
        }
    }
}
