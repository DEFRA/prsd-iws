namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationComment : Entity
    {
        public Guid NotificationId { get; private set; }
        public string UserId { get; private set; }
        public string Comment { get; private set; }
        public int ShipmentNumber { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }

        protected NotificationComment()
        {
        }

        public NotificationComment(Guid notificationId, string userId, string comment, int shipmentNumber, DateTimeOffset dateAdded)
        {
            this.NotificationId = notificationId;
            this.UserId = userId;
            this.Comment = comment;
            this.ShipmentNumber = shipmentNumber;
            this.DateAdded = dateAdded;
        }
    }
}
