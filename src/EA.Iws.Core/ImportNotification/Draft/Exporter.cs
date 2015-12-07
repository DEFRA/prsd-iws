namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class Exporter : IDraftEntity
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public Contact Contact { get; set; }

        public Guid ImportNotificationId { get; private set; }

        internal Exporter()
        {
        }

        public Exporter(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}