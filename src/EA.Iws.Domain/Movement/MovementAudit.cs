namespace EA.Iws.Domain.Movement
{
    using System;
    using Prsd.Core.Domain;

    public class MovementAudit : Entity
    {
        public Guid NotificationId { get; private set; }
        public int ShipmentNumber { get; private set; }
        public string UserId { get; private set; }
        public int Type { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }

        public MovementAudit(Guid notificationId, int shipmentNumber, string userId, int type, DateTimeOffset dateAdded)
        {
            NotificationId = notificationId;
            ShipmentNumber = shipmentNumber;
            UserId = userId;
            Type = type;
            DateAdded = dateAdded;
        }

        protected MovementAudit()
        {
        }
    }
}
