namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using Shared;

    public class Facility : IDraftEntity
    {
        public Guid Id { get; set; }

        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public BusinessType? Type { get; set; }

        public string RegistrationNumber { get; set; }

        public Contact Contact { get; set; }

        public bool IsActualSite { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public Facility(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal Facility()
        {
        }
    }
}
