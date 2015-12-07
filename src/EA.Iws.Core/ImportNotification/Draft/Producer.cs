using System;

namespace EA.Iws.Core.ImportNotification.Draft
{
    public class Producer : IDraftEntity
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public Contact Contact { get; set; }

        public bool AreMultiple { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public Producer(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal Producer()
        {
        }
    }
}
