namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Requests.ImportMovement;

    internal class ImportMovementAuditMap : IMap<AuditImportMovement, ImportMovementAudit>
    {
        public ImportMovementAudit Map(AuditImportMovement source)
        {
            return new ImportMovementAudit(source.NotificationId,
                source.ShipmentNumber,
                source.UserId,
                (int)source.Type,
                source.DateAdded);
        }
    }
}
