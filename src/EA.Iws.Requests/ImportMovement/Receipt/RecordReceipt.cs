namespace EA.Iws.Requests.ImportMovement.Receipt
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class RecordReceipt : IRequest<bool>
    {
        public Guid ImportMovementId { get; private set; }

        public DateTime Date { get; private set; }

        public ShipmentQuantityUnits Unit { get; private set; }

        public decimal Quantity { get; private set; }

        public RecordReceipt(Guid importMovementId, 
            DateTime date, 
            ShipmentQuantityUnits unit, 
            decimal quantity)
        {
            ImportMovementId = importMovementId;
            Date = date;
            Unit = unit;
            Quantity = quantity;
        }
    }
}
