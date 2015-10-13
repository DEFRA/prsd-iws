namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class MovementOperationReceipt : Entity
    {
        protected MovementOperationReceipt()
        {
        }

        internal MovementOperationReceipt(DateTime dateComplete)
        {
            Date = dateComplete;
        }

        public DateTime Date { get; internal set; }

        public Guid? FileId { get; internal set; }

        public void SetCertificateFile(Guid fileId)
        {
            FileId = fileId;
        }
    }
}