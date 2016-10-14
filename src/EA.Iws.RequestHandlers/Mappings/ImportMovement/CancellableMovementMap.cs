namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;

    internal class CancellableMovementMap : IMap<ImportMovement, ImportCancellableMovement>
    {
        public ImportCancellableMovement Map(ImportMovement source)
        {
            return new ImportCancellableMovement
            {
                MovementId = source.Id,
                Number = source.Number,
                NotificationId = source.NotificationId
            };
        }
    }
}