namespace EA.Iws.Api.Client.Entities
{
    using System;

    public class NotificationInformation
    {
        public NotificationInformation(Guid id, string notificationNumber)
        {
            Id = id;
            NotificationNumber = notificationNumber;
        }

        public Guid Id { get; private set; }

        public string NotificationNumber { get; private set; }
    }
}