namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Exporter notifier")]
    public class Exporter : IDraftEntity
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public Contact Contact { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public bool IsAddedToAddressBook { get; set; }

        internal Exporter()
        {
        }

        public Exporter(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}