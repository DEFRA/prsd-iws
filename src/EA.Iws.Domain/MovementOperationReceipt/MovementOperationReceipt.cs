namespace EA.Iws.Domain.MovementOperationReceipt
{
    using System;
    using Prsd.Core.Domain;

    public class MovementOperationReceipt : Entity
    {
        protected MovementOperationReceipt()
        {
        }

        internal MovementOperationReceipt(DateTime dateReceived)
        {
            Date = dateReceived;
        }

        public DateTime Date { get; internal set; }
    }
}
