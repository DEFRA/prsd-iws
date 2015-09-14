namespace EA.Iws.Domain.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Movement;
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

        public DateTime Date { get; private set; }

        public Decision? Decision { get; internal set; }

        public string RejectReason { get; internal set; }

        public decimal? Quantity { get; internal set; }
    }
}
