namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class HasExceededConsentedQuantity : IRequest<bool>
    {
        public HasExceededConsentedQuantity(Guid notificationId, decimal quantity, ShipmentQuantityUnits units)
        {
            NotificationId = notificationId;
            Quantity = quantity;
            Units = units;
        }

        public Guid NotificationId { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }
    }
}