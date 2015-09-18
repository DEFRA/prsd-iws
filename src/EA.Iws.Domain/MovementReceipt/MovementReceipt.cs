namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using MovementOperationReceipt;
    using Prsd.Core.Domain;

    public class MovementReceipt : Entity
    {
        protected MovementReceipt()
        {
        }

        internal MovementReceipt(DateTime dateReceived)
        {
            Date = dateReceived;
        }
        
        public DateTime Date { get; internal set; }

        public Decision? Decision { get; internal set; }

        public string RejectReason { get; internal set; }

        public decimal? Quantity { get; internal set; }

        public virtual MovementOperationReceipt OperationReceipt { get; private set; }

        public void SetQuantity(decimal quantity)
        {
            if (Decision.HasValue && Decision.Value == Core.MovementReceipt.Decision.Accepted)
            {
                Quantity = quantity;
            }
            else
            {
                throw new InvalidOperationException(
                    "Cannot set quantity for a movement receipt where the movement has not been accepted. Receipt: "
                    + Id);
            }
        }

        public void Reject(string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new InvalidOperationException("A rejection reason must be provided for a rejected shipment");
            }

            if (!string.IsNullOrEmpty(reason) && reason.Length > 200)
            {
                throw new InvalidOperationException("The rejection reason must not be greater than 200 characters in length");
            }

            Decision = Core.MovementReceipt.Decision.Rejected;
            RejectReason = reason;
        }

        public void Accept()
        {
            Decision = Core.MovementReceipt.Decision.Accepted;
            RejectReason = null;
        }
    }
}
