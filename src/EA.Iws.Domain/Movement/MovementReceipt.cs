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

        internal MovementReceipt(Guid fileId, DateTime dateReceived, string rejectionReason)
        {
            FileId = fileId;
            Date = dateReceived;
            RejectReason = rejectionReason;
        }

        internal MovementReceipt(Guid fileId, DateTime dateReceived, decimal quantity)
        {
            FileId = fileId;
            Date = dateReceived;
            Quantity = quantity;
        }

        internal MovementReceipt(DateTime dateReceived, decimal quantity)
        {
            Date = dateReceived;
            Quantity = quantity;
        }
        
        public DateTime Date { get; private set; }

        public Decision? Decision { get; internal set; }

        public string RejectReason { get; internal set; }

        public decimal? Quantity { get; internal set; }

        public Guid? FileId { get; private set; }

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

        public void SetCertificateFile(Guid fileId)
        {
            FileId = fileId;
        }
    }
}
