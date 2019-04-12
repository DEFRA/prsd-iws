namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Prsd.Core.Domain;

    public class ImportMovementAudit : Entity
    {
        public Guid NotificationId { get; private set; }
        public int ShipmentNumber { get; private set; }
        public string UserId { get; private set; }
        public int Type { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }

        public ImportMovementAudit(Guid notificationId, int shipmentNumber, string userId, int type, DateTimeOffset dateAdded)
        {
            NotificationId = notificationId;
            ShipmentNumber = shipmentNumber;
            UserId = userId;
            Type = type;
            DateAdded = dateAdded;
        }

        protected ImportMovementAudit()
        {
        }
    }
}
