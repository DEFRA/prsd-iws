namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using Core.ImportMovement;
    using Prsd.Core.Mapper;
    using ImportMovement = Domain.ImportMovement.ImportMovement;

    internal class MovementDataMap : IMap<ImportMovement, ImportMovementData>
    {
        public ImportMovementData Map(ImportMovement source)
        {
            return new ImportMovementData
            {
                NotificationId = source.NotificationId,
                Number = source.Number,
                ActualDate = source.ActualShipmentDate,
                PreNotificationDate = source.PrenotificationDate
            };
        }
    }
}
