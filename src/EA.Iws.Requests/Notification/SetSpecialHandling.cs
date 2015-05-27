namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class SetSpecialHandling : IRequest<string>
    {
        public SetSpecialHandling(Guid notificationId, bool isSpecialHandling, string specialHandlingDetails)
        {
            NotificationId = notificationId;
            IsSpecialHandling = isSpecialHandling;
            SpecialHandlingDetails = specialHandlingDetails;
        }

        public bool IsSpecialHandling { get; set; }

        public Guid NotificationId { get; private set; }

        public string SpecialHandlingDetails { get; private set; }
    }
}