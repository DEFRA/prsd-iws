namespace EA.Iws.Requests.Movement.Receive
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class RecordReceiptInternal : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public DateTime ReceivedDate { get; private set; }
   
        public decimal ActualQuantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public RecordReceiptInternal(Guid movementId, DateTime receivedDate, decimal actualQuantity, ShipmentQuantityUnits units)
        {
            MovementId = movementId;
            ReceivedDate = receivedDate;
            ActualQuantity = actualQuantity;
            Units = units;
        }
    }
}