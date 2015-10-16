namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class MovementCompletedReceipt : Entity
    {
        protected MovementCompletedReceipt()
        {
        }

        internal MovementCompletedReceipt(DateTime dateComplete, Guid fileId)
        {
            Date = dateComplete;
            FileId = fileId;
        }

        public DateTime Date { get; private set; }

        public Guid? FileId { get; private set; }
    }
}