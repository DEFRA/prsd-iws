namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;
    using Shared;

    [DisplayName("Importer consignee")]
    public class Importer : IDraftEntity
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public BusinessType? Type { get; set; }

        public string RegistrationNumber { get; set; }

        public Contact Contact { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public Importer(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal Importer()
        {
        }
    }
}