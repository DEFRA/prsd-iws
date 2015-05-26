namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class SetSpecialHandling : IRequest<string>
    {
        public SetSpecialHandling(Guid notificationId, bool isSpecialHandling)
        {
            NotificationId = notificationId;
            IsSpecialHandling = isSpecialHandling;
        }

        public bool IsSpecialHandling { get; set; }

        public Guid NotificationId { get; private set; }
    }
}