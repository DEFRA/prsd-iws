namespace EA.Iws.Domain.Movement
{
    using System;
    using Core.MovementReceipt;
    using Prsd.Core.Domain;

    public class MovementReceipt : Entity
    {
        protected MovementReceipt()
        {
        }

        internal MovementReceipt(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
        {
            FileId = fileId;
            Date = dateReceived;
            QuantityReceived = quantity;
            CreatedBy = createdBy.ToString();
        }

        internal MovementReceipt(DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
        {
            Date = dateReceived;
            QuantityReceived = quantity;
            CreatedBy = createdBy.ToString();
        }
        
        public DateTime Date { get; private set; }

        public Decision? Decision { get; private set; }

        public ShipmentQuantity QuantityReceived { get; private set; }

        public Guid? FileId { get; private set; }

        public string CreatedBy { get; private set; }
    }
}
