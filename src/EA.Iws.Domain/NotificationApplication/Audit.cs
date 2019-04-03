namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class Audit : Entity
    {
        protected Audit()
        {
        }

        public Audit(Guid notificationId, string userId, int screen, int type, DateTimeOffset dateAdded)
        {
            this.NotificationId = notificationId;
            this.UserId = userId;
            this.Screen = screen;
            this.Type = type;
            this.DateAdded = dateAdded;
        }

        public Guid NotificationId { get; private set; }
        public string UserId { get; private set; }
        public int Screen { get; private set; }
        public int Type { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }
    }
}
