namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class MovementCompletedReceipt : Entity
    {
        protected MovementCompletedReceipt()
        {
        }

        internal MovementCompletedReceipt(DateTime dateComplete, Guid fileId, Guid createdBy)
        {
            Date = dateComplete;
            FileId = fileId;
            CreatedBy = createdBy.ToString();
        }

        internal MovementCompletedReceipt(DateTime dateComplete, Guid createdBy)
        {
            Date = dateComplete;
            CreatedBy = createdBy.ToString();
        }

        public DateTime Date { get; private set; }

        public Guid? FileId { get; private set; }

        public string CreatedBy { get; private set; }
    }
}