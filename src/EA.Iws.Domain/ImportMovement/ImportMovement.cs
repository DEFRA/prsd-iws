namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core.Domain;

    public class ImportMovement : Entity
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        internal ImportMovement(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }

        private ImportMovement()
        {
        }
    }
}
